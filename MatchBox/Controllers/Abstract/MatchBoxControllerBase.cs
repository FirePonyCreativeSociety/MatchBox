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
        protected MatchBoxControllerBase(SecurityConfiguration config)
            : base()
        {
            SystemConfiguration = config;            
        }

        protected SecurityConfiguration SystemConfiguration { get; }

        protected bool CheckAdmin(string adminKeyValue)
        {
            return CheckAdminWhen(() => true, adminKeyValue);
        }

        protected bool CheckAdminWhen(bool check, string adminKeyValue)
        {
            return CheckAdminWhen(() => check, adminKeyValue);
        }

        protected bool CheckAdminWhen(Func<bool> check, string adminKeyValue)
        {
            if (check == null)
                throw new ArgumentNullException(nameof(check));

            if (!check())
                return false;
            else
                return (adminKeyValue == SystemConfiguration.AdminKey);
        }       
    }
}
