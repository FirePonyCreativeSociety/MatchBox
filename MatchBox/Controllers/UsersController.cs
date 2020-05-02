using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data;
using MatchBox.Data.Extensions;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
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
            SignInManager<DbUser> signInManager,
            IEmailSender emailSender)
            : base(dbContext, mapper)
        {
            JwtProducer = jwtProducer ?? throw new ArgumentNullException();
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
        }

        public IJwtProducer JwtProducer { get; }
        public UserManager<DbUser> UserManager { get; }
        public SignInManager<DbUser> SignInManager { get; }
        public IEmailSender EmailSender { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;

        async Task<DbUser> FindUserByUsernameOrEmail(string usernameOrEmail)
        {
            var user = await UserManager.FindByNameAsync(usernameOrEmail);
            return user ?? await UserManager.FindByEmailAsync(usernameOrEmail);
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await FindUserByUsernameOrEmail(model.UsernameOrEmail);
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

        // https://code-maze.com/password-reset-aspnet-core-identity/
        [AllowAnonymous]
        [HttpPost(nameof(ForgotPassword))]
        public async Task<IActionResult> ForgotPassword([FromBody] UsernameOrEmailModel model)
        {
            var user = await FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (user == null)
                return Ok();

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), nameof(UsersController), new { token, email = user.Email }, Request.Scheme);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback);
            await EmailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost(nameof(ResetPassword))]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var user = await FindUserByUsernameOrEmail(resetPasswordModel.Email);
            if (user == null)
                return Ok();

            var resetPassResult = await UserManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}