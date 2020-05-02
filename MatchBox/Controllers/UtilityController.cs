using MatchBox.Configuration;
using MatchBox.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class UtilityController : MatchBoxControllerBase
    {
        public UtilityController(MatchBoxConfiguration configuration)
            : base()
        {
            Configuration = configuration;
        }

        public MatchBoxConfiguration Configuration { get; }

        [AllowAnonymous]
        [HttpGet()]
        public IActionResult Get()
        {
            var tmp = new 
            { 
                PasswordSettings = Configuration.Password,
                UserSettings = Configuration.User
            };

            return Ok(tmp);
        }
    }
}
