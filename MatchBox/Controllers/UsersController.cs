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
    // https://code-maze.com/user-lockout-aspnet-core-identity/

    public class UsersController : RESTControllerBase<User, DbUser>
    {
        private const string BadLoginMessage = "Username or password is incorrect";
        private const string UserLockedOutMessage = "User is locked out.";

        public UsersController(MatchBoxDbContext dbContext, IMapper mapper, IJwtProducer jwtProducer, 
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager)
            : base(dbContext, mapper)
        {
            JwtProducer = jwtProducer;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IJwtProducer JwtProducer { get; }
        public UserManager<DbUser> UserManager { get; }
        public SignInManager<DbUser> SignInManager { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await UserManager.FindByNameAsync(model.UsernameOrEmail);
            user = user ?? await UserManager.FindByEmailAsync(model.UsernameOrEmail);
            if (user == null)
                return BadRequest(new { message = BadLoginMessage });

            var result = await SignInManager.PasswordSignInAsync(user, model.Password, false, true);

            if (result.IsLockedOut)
                return BadRequest(new { message = UserLockedOutMessage });            
            
            if (!result.Succeeded)
                return BadRequest(new { message = BadLoginMessage });                     

            var jwt = JwtProducer.Generate(user);
            
            return Ok(jwt);
        }
    }
}