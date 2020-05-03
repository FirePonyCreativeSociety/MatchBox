using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Internal;
using MatchBox.Services.Email;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    // https://code-maze.com/user-lockout-aspnet-core-identity/

    public class UsersController : RESTControllerBase<User, DbUser>
    {        
        public UsersController(
            MatchBoxDbContext dbContext, 
            IMapper mapper, 
            IJwtProducer jwtProducer, 
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
        public EmailConfiguration EmailConfiguration { get; }

        protected override IQueryable<DbUser> ControllerDbSet => DbContext.Users;

        async Task<DbUser> FindUserByUsernameOrEmail(string usernameOrEmail)
        {
            var user = await UserManager.FindByNameAsync(usernameOrEmail);
            return user ?? await UserManager.FindByEmailAsync(usernameOrEmail);
        }        
    }
}