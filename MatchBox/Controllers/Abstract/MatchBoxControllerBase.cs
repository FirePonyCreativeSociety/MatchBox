using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors()]
    [Route("api/[controller]")]
    public abstract class MatchBoxControllerBase : ControllerBase
    {
        protected string Header_AccessLevel = Version.ProductNameNoSpacesFor("AccessLevel"); // MatchBox.AccessLevel
        protected const string AccessLevel_Admin = "ADMIN";

        protected MatchBoxControllerBase(IDataProtectionProvider dataProtectionProvider)
            : base()
        {
            AdminProtector = dataProtectionProvider.CreateProtector(Version.ProductNameNoSpacesFor(Header_AccessLevel)) ?? throw new ArgumentNullException(nameof(dataProtectionProvider));
        }

        protected IDataProtector AdminProtector { get; }

        protected bool EnsureIsAdmin<MODEL>(out ActionResult<MODEL> mandatoryResponse)
            where MODEL : class
        {            
            return EnsureHasAccessLevel<MODEL>(AccessLevel_Admin, out mandatoryResponse);
        }

        protected bool EnsureHasAccessLevel<MODEL>(string requiredAccessLevelId, out ActionResult<MODEL> mandatoryResponse)
            where MODEL : class
        {
            if (requiredAccessLevelId == null)
                throw new ArgumentNullException(nameof(requiredAccessLevelId));
            
            mandatoryResponse = default;

            try
            {                
                if (!Request.Headers.TryGetValue(Header_AccessLevel, out var encryptedAccessLevels))
                {
                    mandatoryResponse = Unauthorized($"MISSING_HEADER_{Header_AccessLevel}");
                    return false;
                }

                var legitAccessLevels = AdminProtector?.Unprotect(encryptedAccessLevels)
                                                       .Split();
                var hasLevel = legitAccessLevels.Contains(requiredAccessLevelId);
                if (!hasLevel)
                {
                    mandatoryResponse = Unauthorized($"!{requiredAccessLevelId}");
                    return false;
                }

                return true;
            }
            catch
            {
                mandatoryResponse = Unauthorized("BAD_KEY");

                return false;
            }
        }

    }
}
