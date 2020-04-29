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
        public UtilityController(MatchBoxSettings settings)
            : base()
        {
            Settings = settings;
        }

        public MatchBoxSettings Settings { get; }

        [AllowAnonymous]
        [HttpGet()]
        public IActionResult Get()
        {
            var tmp = new 
            { 
                PasswordSettings = Settings.Password,
                UserSettings = Settings.User
            };

            return Ok(tmp);
        }
    }
}
