using MatchBox.Data.Extensions;
using MatchBox.Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Data
{
    public static class MatchBoxDbInitializer
    {
        public static async Task Initialize(this MatchBoxDbContext context) 
        {
            context.Database.EnsureCreated();

            // If no users are present we create all the default entities
            if (await context.Users.AnyAsync())
                return;

            // Adds the default groups
            var defGroups = GenerateDefaultGroups(context);
            await context.AddRangeAsync(defGroups);

            // Adds the default users (this includes the admin user)
            var users = GenerateDefaultUsersAndGroupLinks(context, defGroups);
            await context.AddRangeAsync(users);

            // Commits
            await context.SaveChangesAsync();
        }

        #region Factory methods for default users

        const string AleFUserName = "AleF";
        const string ZacUserName = "Zac";
        const string MrKUserName = "MrK";
        const string MetaUserName = "Meta";
        const string GrinsUserName = "Grins";

        static IEnumerable<DbUser> GenerateDefaultUsersAndGroupLinks(MatchBoxDbContext context, IEnumerable<DbGroup> defaultGroups)
        {
            var g_admin = defaultGroups.Single(g => g.Name == AdminGroupName);
            var g_sonofabit = defaultGroups.Single(g => g.Name == SonOfABitGroupName);
            var g_edmcafe = defaultGroups.Single(g => g.Name == EdmCafeGroupName);
            var g_sanct = defaultGroups.Single(g => g.Name == SanctuaryBitGroupName);
            var g_moder = defaultGroups.Single(g => g.Name == ModerangersGroupName);

            var list = new List<DbUser>();

            // Admin
            var admin = list.AddAndReturn(new DbUser
            {
                UserName = MatchBoxDbContext.AdminUserName,
                Email = "me@thisserver.net",
                FirstName = "Admin",                
            });
            admin.UserGroups.Add(new DbUserGroup { Group = g_admin}); // The Group is sufficient. No need to set the User.

            // AleF
            var alef = list.AddAndReturn(new DbUser
            {
                UserName = AleFUserName,
                Email = "myemail@gmail.com",
                FirstName = "Federico",
                MiddleName = "Alessandro",
                LastName = "Federici",
            });
            alef.UserGroups.Add(new DbUserGroup { Group = g_admin});
            alef.UserGroups.Add(new DbUserGroup { Group = g_sonofabit });

            // Zac
            var zac = list.AddAndReturn(new DbUser
            {
                UserName = ZacUserName,
                Email = "zak@lucasarts.net",
                FirstName = "Zac",
                LastName = "McKracken",
            });
            zac.UserGroups.Add(new DbUserGroup { Group = g_admin });
            zac.UserGroups.Add(new DbUserGroup { Group = g_edmcafe });

            // MrK
            var mrk = list.AddAndReturn(new DbUser
            {
                UserName = MrKUserName,
                Email = "keith@thisserver.com",
                FirstName = "Keith",
                LastName = "Longbeard",
            });
            mrk.UserGroups.Add(new DbUserGroup { Group = g_admin });

            // Meta
            var meta = list.AddAndReturn(new DbUser
            {
                UserName = MetaUserName,
                Email = "scott@thatserver.net",
                FirstName = "Scott",
                LastName = "Meta",            
            });
            meta.UserGroups.Add(new DbUserGroup { Group = g_sanct });
            meta.UserGroups.Add(new DbUserGroup { Group = g_moder });

            // Grins
            var grins = list.AddAndReturn(new DbUser
            {
                UserName = GrinsUserName,
                Email = "diana@whateverserver.net",
                FirstName = "Diana",
                LastName = "Happy",
            });
            grins.UserGroups.Add(new DbUserGroup { Group = g_sanct });
            grins.UserGroups.Add(new DbUserGroup { Group = g_moder });

            //// Bunch of other random users
            //for (int i = 0; i < 300; i++)
            //{
            //    var strId = i.ToString("D3");
            //    list.Add(new User
            //    {
            //        UserName = $"User{strId}",
            //        Email = $"emailOfUser{strId}@noserver.net",
            //        FirstName = "User",
            //        LastName = strId,
            //    });
            //}

            return list;
        }

        const string SonOfABitGroupName = "Son Of a Bit";
        const string EdmCafeGroupName = "EDM Cafe";
        const string SanctuaryBitGroupName = "Sanctuary";
        const string AdminGroupName = "Administrators";
        const string ModerangersGroupName = "Moderangers";

        static IEnumerable<DbGroup> GenerateDefaultGroups(MatchBoxDbContext context)
        {
            var list = new List<DbGroup>();

            list.Add(new DbGroup
            {
                Name = AdminGroupName,
                Description = "Administrators only.",
            });

            list.Add(new DbGroup
            { 
                Name = SonOfABitGroupName,
                Description = "Arcades of the 80s and 90s are back! Join us for some digital fun.",                                
            });

            list.Add(new DbGroup
            {
                Name = EdmCafeGroupName,
                Description = "Great EDM music playing all night long! BYOB obviously.",
            });

            list.Add(new DbGroup
            {
                Name = SanctuaryBitGroupName,
                Description = "The right place to go when you the inner pug goes on vacation.",
            });

            list.Add(new DbGroup
            {
                Name = ModerangersGroupName,
                Description = "The heroes that make sure bad things don't happen.",
            });

            return list;
        }

        //static void LinkDefaultGroupsAndUsers(IEnumerable<User> users, IEnumerable<Group> groups)
        //{
        //    //var u_admin = users.Single(u => u.UserName == MatchBoxDbContext.AdminUserName);
        //    var u_alef = users.Single(u => u.UserName==AleFUserName);
        //    var u_zac = users.Single(u => u.UserName == ZacUserName);                        
        //    var u_mrk = users.Single(u => u.UserName == MrKUserName);
        //    var u_meta = users.Single(u => u.UserName == MetaUserName);
        //    var u_grins = users.Single(u => u.UserName == GrinsUserName);
        //
        //    var g_admin = groups.Single(g => g.Name == AdminGroupName);
        //    var g_sonofabit = groups.Single(g => g.Name == SonOfABitGroupName);
        //    var g_edmcafe = groups.Single(g => g.Name == SonOfABitGroupName);
        //    var g_sanct = groups.Single(g => g.Name == SonOfABitGroupName);
        //    var g_rangers = groups.Single(g => g.Name == ModerangersGroupName);
        //
        //
        //}

        #endregion        
    }
}
