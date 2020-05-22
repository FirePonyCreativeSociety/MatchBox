using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Configuration;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Models;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : RESTControllerBase<User, DbUser>
    {
        const string BadLoginMessage = "Username or password is incorrect";
        const string UserLockedOutMessage = "User is locked out.";
        const string CredentialsAlreadyTaken  = "The user name or email is already taken.";

        public AuthenticationController(
            MatchBoxDbContext dbContext, 
            IMapper mapper, 
            IJwtProducer jwtProducer,
            SecurityConfiguration securityConfig,
            UserConfiguration userConfig,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IEmailSender emailSender)
            : base(securityConfig, dbContext, mapper)
        {
            JwtProducer = jwtProducer ?? throw new ArgumentNullException();
            UserConfig = userConfig;
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;            
        }

        public IJwtProducer JwtProducer { get; }
        public UserConfiguration UserConfig { get; }
        public UserManager<DbUser> UserManager { get; }
        public SignInManager<DbUser> SignInManager { get; }
        public IEmailSender EmailSender { get; }
        
        public EmailConfiguration EmailConfiguration { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;
        
        [HttpPost(nameof(Login))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userLookup = await UserManager.FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (!userLookup.Found)
                return BadRequest(new { message = BadLoginMessage });

            var signInResult = await SignInManager.PasswordSignInAsync(userLookup.Value, model.Password, false, true);

            if (signInResult.IsLockedOut)
                return BadRequest(new { message = UserLockedOutMessage });

            if (!signInResult.Succeeded)
                return BadRequest(new { message = BadLoginMessage });

            //var jwt = JwtProducer.Generate(userLookup.Value);

            return Ok(new LoginResponseModel 
            { 
                UserName = userLookup.Value.UserName,
                Email = userLookup.Value.Email,
                Jwt = JwtProducer.Generate(userLookup.Value)
            });
        }
        
        [HttpPost(nameof(ForgotPassword))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword([FromBody] UsernameOrEmailModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // https://code-maze.com/password-reset-aspnet-core-identity/        
            var candidateUser = await UserManager.FindUserByUsernameOrEmail(model.UsernameOrEmail);
            if (!candidateUser.Found)
                return NotFound();

            return Ok(new ForgotPasswordResponse
            {
                Email = candidateUser.Value.Email,
                Token = await UserManager.GeneratePasswordResetTokenAsync(candidateUser.Value)
            });                                                          
        }

        [HttpPost(nameof(ResetPassword))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmptyModel>> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindUserByUsernameOrEmail(model.Email);
            if (!user.Found)
                return NotFound();

            var resetPassResult = await UserManager.ResetPasswordAsync(user.Value, model.Token, model.NewPassword);
            if (resetPassResult.Succeeded)
                return Ok();
            
            // Collects the errors
            foreach (var error in resetPassResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            
            return BadRequest(ModelState);                        
        }

        [HttpPost(nameof(RegisterNewUser))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseModel>> RegisterNewUser(
            [FromBody] RegisterNewUserModel model, 
            [FromHeader(Name = Headers.AdminKey)] string adminKey)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindUserByUsernameOrEmail(model.Email);
            if (user.Found)
                return Conflict(new { message = CredentialsAlreadyTaken });

            var newUser = Mapper.Map<DbUser>(model);
            
            if ((UserConfig.DefaultUserClaims != null) && (UserConfig.DefaultUserClaims.Any()))
            {
                var defClaims = UserConfig.DefaultUserClaims
                                          .Split(';')
                                          .Select(c => c.Split('='));
                
                newUser.Claims ??= new Collection<DbUserClaim>();

                foreach (var defClaim in defClaims)
                {
                    newUser.Claims.Add(new DbUserClaim
                    {
                        ClaimType = defClaim.First().Trim(),
                        ClaimValue = defClaim.Last().Trim()
                    });
                }
            }

            var shouldCheck = (model.Claims != null) && model.Claims.Any();
            if (!CheckAdminWhen(shouldCheck, adminKey))
                return Unauthorized("Only admins can set claims.");
                
            var createUserResult = await UserManager.CreateAsync(newUser, model.Password);
            if (createUserResult.Succeeded)
            {
                return Ok(new LoginResponseModel
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    Jwt = JwtProducer.Generate(newUser)
                });
            }

            // Collects the errors
            foreach (var error in createUserResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}