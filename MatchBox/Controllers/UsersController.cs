using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data;
using MatchBox.Data.Extensions;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class UsersController : RESTControllerBase<User, DbUser>
    {
        private const string BadLoginMessage = "Username or password is incorrect";

        public UsersController(MatchBoxDbContext dbContext, IMapper mapper, IJwtProducer jwtProducer, UserManager<DbUser> userManager)
            : base(dbContext, mapper)
        {
            JwtProducer = jwtProducer;
            UserManager = userManager;
        }

        public IJwtProducer JwtProducer { get; }
        public UserManager<DbUser> UserManager { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationInformation model)
        {
            var user = await UserManager.FindByNameAsync(model.UsernameOrEmail);
            user = user ?? await UserManager.FindByEmailAsync(model.UsernameOrEmail);
            if (user == null)
                return BadRequest(new { message = BadLoginMessage });
            
            var jwt = JwtProducer.Generate(user);
            
            return Ok(jwt);
        }
    }
}