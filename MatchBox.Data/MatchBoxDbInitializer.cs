using MatchBox.Data.Extensions;
using MatchBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Data
{
    public class MatchBoxDbInitializer
    {
        public MatchBoxDbInitializer(IServiceProvider serviceProvider)
            : base()
        {
            serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // TODO: review this. I don't want too many arguments in the ctor, but querying the
            // IServiceProvider directly is not clean...
            Context = serviceProvider.GetRequiredService<MatchBoxDbContext>();
            UserManager = serviceProvider.GetRequiredService<UserManager<DbUser>>();
            RoleManager = serviceProvider.GetRequiredService<RoleManager<DbRole>>();
        }

        public MatchBoxDbContext Context { get; }
        public UserManager<DbUser> UserManager { get; }
        public RoleManager<DbRole> RoleManager { get; }

        public async Task Initialize() 
        {
            Context.Database.EnsureCreated();

            // If no users are present we create all the default entities
            if (await Context.Users.AnyAsync())
                return;

            // Adds the default roles
            await GenerateDefaultRoles();

            // Adds the default users (this includes the admin user)
            await GenerateDefaultUsers();
        }

        #region Factory methods for default users

        const string AleFUserName = "AleF";
        const string ZacUserName = "Zac";
        const string MrKUserName = "MrK";
        const string MetaUserName = "Meta";
        const string GrinsUserName = "Grins";

        async Task AddUser(
            string userName, 
            string email, 
            string firstName,             
            string middleName,
            string lastName,
            params string[] roles)
        {
            var newUser = new DbUser
            {
                UserName = userName,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Email = email,
            };

            var result = await UserManager.CreateAsync(newUser, userName.ToLower() + "_A123");
            if (!result.Succeeded)
                throw new System.Exception($"Could not create the default user '{userName}'.");

            var result2 = await UserManager.AddToRolesAsync(newUser, roles);
            if (!result.Succeeded)
                throw new System.Exception($"Could not add the default roles of user '{userName}'.");
        }

        
        
        async Task GenerateDefaultUsers()
        {
            // Admin
            await AddUser(userName: MatchBoxDbContext.AdminUserName, email: "me@thisserver.net", firstName: "System", middleName: null, lastName: "Administrator",
                          AdminRoleName);

            // AleF
            await AddUser(userName: AleFUserName, email: "myemail@gmail.com", firstName: "Federico", middleName: "Alessandro", lastName: "Federici",
                          AdminRoleName, SonOfABitRoleName);

            // Zac
            await AddUser(userName: ZacUserName, email: "zak@lucasarts.net", firstName: "Zac", middleName: null, lastName: "McKracken",
                          AdminRoleName, EdmCafeRoleName);

            // MrK
            await AddUser(userName: MrKUserName, email: "keith@thisserver.com", firstName: "Keith", middleName: null, lastName: "Longbeard",
                          AdminRoleName);

            // Meta
            await AddUser(userName: MetaUserName, email: "scott@thatserver.net", firstName: "Scott", middleName: null, lastName: "Meta",
                          SanctuaryBitRoleName, ModerangersRoleName);

            // Grins
            await AddUser(userName: GrinsUserName, email: "diana@whateverserver.net", firstName: "Diana", middleName: null, lastName: "Happy",
            SanctuaryBitRoleName, ModerangersRoleName);
            
            ////// Bunch of other random users
            ////for (int i = 0; i < 300; i++)
            ////{
            ////    var strId = i.ToString("D3");
            ////    list.Add(new User
            ////    {
            ////        UserName = $"User{strId}",
            ////        Email = $"emailOfUser{strId}@noserver.net",
            ////        FirstName = "User",
            ////        LastName = strId,
            ////    });
            ////}
            //
            //return list;
        }

        const string AdminRoleName = "Administrators";
        const string SonOfABitRoleName = "Son Of a Bit";
        const string EdmCafeRoleName = "EDM Cafe";
        const string SanctuaryBitRoleName = "Sanctuary";        
        const string ModerangersRoleName = "Moderangers";

        async Task AddRole(string name, string description)
        {
            var newRole = new DbRole
            {
                Name = name,
                Description = description
            };
            
            await RoleManager.CreateAsync(newRole);
        }

        async Task GenerateDefaultRoles()
        {
            await AddRole(AdminRoleName, "Administrators only.");
            await AddRole(SonOfABitRoleName, "Arcades of the 80s and 90s are back! Join us for some digital fun.");
            await AddRole(EdmCafeRoleName, "Great EDM music playing all night long! BYOB obviously.");
            await AddRole(SanctuaryBitRoleName, "The right place to go when you the inner pug goes on vacation.");
            await AddRole(ModerangersRoleName, "The heroes that make sure bad things don't happen.");
        }

        #endregion
    }
}
