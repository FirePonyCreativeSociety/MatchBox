using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Configuration;
using MatchBox.Internal;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class UtilityController : MatchBoxControllerBase
    {
        public UtilityController(MatchBoxConfiguration configuration, IDataProtectionProvider dataProtectionProvider, IEmailSender emailSender, IMapper mapper)
            : base(dataProtectionProvider)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public MatchBoxConfiguration Configuration { get; }
        public IEmailSender EmailSender { get; }
        public IMapper Mapper { get; }

        [AllowAnonymous]
        [HttpGet(nameof(GetSystemInformation))]
        public IActionResult GetSystemInformation()
        {
            var keyLength = Configuration?.Security?.JwtIssuerSigningKey?.Length ?? 0;

            // Really cool way to send back a JSON object after building it anonymously right here!!!
            var result = new 
            { 
                PasswordSettings = Configuration.Password,
                UserSettings = Configuration.User,
                Configuration.Security?.CorsOrigins, // This uses the default name CorsOrigin
                Email = new
                {
                    Configuration.Email?.From,
                    Configuration.Email?.UserName,
                    Configuration.Email?.SmtpServer,
                    Configuration.Email?.Port,
                    Configuration.Email?.UseSSL,
                },
                JwtIssuerSigningKeyLength = keyLength,
                CurrentDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            };

            return Ok(result);
        }

        [HttpPost(nameof(SendEmail))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<EmptyResult>> SendEmail([FromBody] SendEmailModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // If the caller intends to store claims along side the new users the caller must have ADMIN role.
            if (!EnsureIsAdmin<EmptyResult>(out var oopsResponse))
                return oopsResponse;

            var msg = new Message(model.To, model.Subject, model.Content);
            await EmailSender.SendEmailAsync(msg);
            return Ok();
    }
}
}
