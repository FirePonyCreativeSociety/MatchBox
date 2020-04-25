using AutoMapper;
using MatchBox.API.Model;
using MatchBox.Data;
using MatchBox.Data.Extensions;
using MatchBox.Data.Model;
using MatchBox.Internal;
using MatchBox.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class UsersController : RESTControllerBase<User, DbUser>
    {
        private const string BadLoginMessage = "Username or password is incorrect";

        public UsersController(MatchBoxDbContext dbContext, IMapper mapper, IOptions<MatchBoxSettings> settings, IJwtProducer jwtProducer)
            : base(dbContext, mapper)
        {
            Settings = settings.Value;
            JwtProducer = jwtProducer;
        }

        public MatchBoxSettings Settings { get; }
        public IJwtProducer JwtProducer { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationInformation model)
        {
            var tmp = await DbContext.TryFindUserByName(model.Username);
            if (!tmp.Found)
                return BadRequest(new { message = BadLoginMessage });

            var jwt = JwtProducer.Generate(tmp.Value);            
            return Ok(jwt);
        }
    }
}