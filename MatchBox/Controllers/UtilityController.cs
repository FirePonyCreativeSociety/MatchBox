using MatchBox.API.Models;
using MatchBox.Configuration;
using MatchBox.Internal;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class UtilityController : MatchBoxControllerBase
    {
        public UtilityController(MatchBoxConfiguration configuration, IEmailSender emailSender)
            : base()
        {
            Configuration = configuration;
            EmailSender = emailSender;
        }

        public MatchBoxConfiguration Configuration { get; }
        public IEmailSender EmailSender { get; }

        [AllowAnonymous]
        [HttpGet()]
        public IActionResult Get()
        {
            var tmp = new 
            { 
                PasswordSettings = Configuration.Password,
                UserSettings = Configuration.User,
                Configuration.Security?.CorsOrigins // This uses the default name CorsOrigin
            };

            return Ok(tmp);
        }

        [HttpPost(nameof(SendEmail))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> SendEmail([FromBody] SendEmailModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var msg = new Message(model.To, model.Subject, model.Content);
            await EmailSender.SendEmailAsync(msg);
            return Ok();
    }
}
}
