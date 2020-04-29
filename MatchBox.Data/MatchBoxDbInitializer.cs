using MatchBox.Data.Extensions;
using MatchBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            // Adds the default roles
            await GenerateDefaultGroups();

            // Adds the default users (this includes the admin user)
            await GenerateDefaultUsers();
        }

        const string AleFUserName = "AleF";
        const string ZacUserName = "Zac";
        const string MrKUserName = "MrK";
        const string MetaUserName = "Meta";
        const string GrinsUserName = "Grins";

        async Task AddUser(
            string userName, 
            string email, 
            DateTime dateOfBirth,
            string firstName,             
            string middleName,
            string lastName,
            string[] roles,
            string[] groups)
        {            
            var newUser = new DbUser
            {
                UserName = userName,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Email = email,
                DateOfBirth = dateOfBirth
            };

            var result = await UserManager.CreateAsync(newUser, "password");
            if (!result.Succeeded)
                throw new System.Exception($"Could not create the default user '{userName}'.");

            groups = groups ?? new string[0];
            if (groups.Any())
            {
                newUser.UserGroups = new Collection<DbUserGroup>();
                var targetGroups = Context.Groups.Where(g => groups.Contains(g.Name)).ToList();
                foreach (var grp in targetGroups)
                {
                    newUser.UserGroups.Add(new DbUserGroup 
                    { 
                        GroupId = grp.GroupId,
                        UserId = newUser.Id
                    });
                }
            }

            var result2 = await UserManager.AddToRolesAsync(newUser, roles);
            if (!result.Succeeded)
                throw new System.Exception($"Could not add the default roles of user '{userName}'.");
        }

        
        
        async Task GenerateDefaultUsers()
        {
            // Admin
            await AddUser(userName: MatchBoxDbContext.AdminUserName, email: "me@thisserver.net", dateOfBirth: DateTime.Now, firstName: "System", middleName: null, lastName: "Administrator",
                          new[] { AdminRoleName },
                          null);

            // AleF
            await AddUser(userName: AleFUserName, email: "myemail@gmail.com", dateOfBirth: new DateTime(1975, 12, 8), firstName: "Federico", middleName: "Alessandro", lastName: "Federici",
                          new[] { AdminRoleName }, 
                          new[] { SonOfABitGroupName });

            // Zac
            await AddUser(userName: ZacUserName, email: "zak@lucasarts.net", dateOfBirth: new DateTime(1985, 2, 3), firstName: "Zac", middleName: null, lastName: "McKracken",
                          new[] { AdminRoleName }, 
                          new [] { EdmCafeGroupName });

            // MrK
            await AddUser(userName: MrKUserName, email: "keith@thisserver.com", dateOfBirth: new DateTime(1975, 1, 2), firstName: "Keith", middleName: null, lastName: "Longbeard",
                          new[] { AdminRoleName },
                          null);

            // Meta
            await AddUser(userName: MetaUserName, email: "scott@thatserver.net", dateOfBirth: new DateTime(1970, 5, 12), firstName: "Scott", middleName: null, lastName: "Meta",                           
                          new[] { ModerangersRoleName },
                          new[] { SanctuaryBitGroupName });

            // Grins
            await AddUser(userName: GrinsUserName, email: "diana@whateverserver.net", dateOfBirth: new DateTime(1973, 7, 13), firstName: "Diana", middleName: null, lastName: "Happy",
                          new[] { ModerangersRoleName },
                          new[] { SanctuaryBitGroupName });
            
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
        const string ModerangersRoleName = "Moderangers";

        async Task AddRole(string name, string description)
        {
            var newRole = new DbRole
            {
                Name = name,
                Description = description,
                RoleClaims = new List<DbRoleClaim>()
            };
            
            await RoleManager.CreateAsync(newRole);
        }

        async Task GenerateDefaultRoles()
        {
            await AddRole(AdminRoleName, "Administrators only.");
            await AddRole(ModerangersRoleName, "The heroes that make sure bad things don't happen.");
        }


        const string SonOfABitGroupName = "Son Of a Bit";
        const string EdmCafeGroupName = "EDM Cafe";
        const string SanctuaryBitGroupName = "Sanctuary";

        async Task AddGroup(string name, string description)
        {
            var newGroup = new DbGroup
            {
                Name = name,
                Description = description,
                Type = "THEMECAMP"
            };

            await Context.AddAsync(newGroup);
        }

        async Task GenerateDefaultGroups()
        {
            await Task.WhenAll(
                AddGroup(SonOfABitGroupName, "Arcades of the 80s and 90s are back! Join us for some digital fun."),
                AddGroup(EdmCafeGroupName, "Great EDM music playing all night long! BYOB obviously."),
                AddGroup(SanctuaryBitGroupName, "The right place to go when you the inner pug goes on vacation.")
               );

            await Context.SaveChangesAsync();
        }

    }
}
