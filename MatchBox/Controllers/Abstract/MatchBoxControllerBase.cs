using MatchBox.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MatchBox.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [EnableCors()]
    [Route("api/[controller]")]
    public abstract class MatchBoxControllerBase : ControllerBase
    {
        protected MatchBoxControllerBase(SecurityConfiguration securityConfig)
            : base()
        {
            SystemConfiguration = securityConfig;            
        }

        protected SecurityConfiguration SystemConfiguration { get; }

        protected bool CheckAdmin(string adminKeyValue)
        {
            var res = CheckAdminWhen(() => true, adminKeyValue);
            return res;
        }

        protected bool CheckAdminWhen(bool check, string adminKeyValue)
        {
            var res = CheckAdminWhen(() => check, adminKeyValue);
            return res;
        }

        protected bool CheckAdminWhen(Func<bool> check, string adminKeyValue)
        {
            if (check == null)
                throw new ArgumentNullException(nameof(check));

            var shouldCheck = check();
            if (shouldCheck)
            {
                var res = adminKeyValue == SystemConfiguration.AdminKey;
                return res;
            }
            else
                return true;
        }       
    }
}
