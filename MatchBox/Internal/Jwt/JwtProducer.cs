using MatchBox.Configuration;
using MatchBox.Data.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MatchBox.Internal
{
    public class JwtProducer : JwtProducerBase
    {
        public JwtProducer(IOptions<MatchBoxConfiguration> settings)
            : base(settings)
        {
        }

        public override string Generate(DbUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.Security.JwtIssuerSigningKey);

            var claimsList = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString()),
            };

            if (user.Claims != null)
            {
                foreach (var claim in user.Claims)
                {
                    claimsList.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsList),
                Audience = Configuration.Security.Audience,
                Issuer =  Configuration.Security.Issuer,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt =  tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
