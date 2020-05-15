using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Configuration;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class UtilityController : MatchBoxControllerBase
    {
        public UtilityController(SecurityConfiguration config, MatchBoxConfiguration configuration, IEmailSender emailSender, IMapper mapper)
            : base(config)
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
            var jwtKeyLength = Configuration?.Security?.JwtIssuerSigningKey?.Length ?? 0;
            var adminKeyLengthPresent = !string.IsNullOrWhiteSpace(Configuration?.Security?.AdminKey);

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
                JwtIssuerSigningKeyLength = jwtKeyLength,
                AdminKeyPresent = adminKeyLengthPresent,
                CurrentDateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
            };

            return Ok(result);
        }

        [HttpPost(nameof(SendEmail))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> SendEmail(
            [FromBody] SendEmailModel model,
            [FromHeader(Name = Headers.AdminKey)] string adminKey)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!CheckAdmin(adminKey))
                return Unauthorized();

            var msg = new Message(model.To, model.Subject, model.Content);
            await EmailSender.SendEmailAsync(msg);
            return Ok();
    }
}
}
