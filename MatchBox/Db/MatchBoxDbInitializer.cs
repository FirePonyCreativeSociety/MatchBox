using MatchBox.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Db
{
    public static class MatchBoxDbInitializer
    {
        public static async Task Initialize(this MatchBoxContext context) 
        {
            context.Database.EnsureCreated();

            // If the admin is not there we need to create all the default entities
            if (!await CreateDefaultAdminIfNecessary(context))
                return;

            // Adds the default users
            await context.AddRangeAsync(GenerateDefaultUsers(context));

            // Commits
            await context.SaveChangesAsync();

        }

        #region Factory methods for default users

        static User GenerateDefaultAdmin()
        {
            return new User
            {
                UserName = MatchBoxContext.AdminUserName,
            };
        }

        static IEnumerable<User> GenerateDefaultUsers(MatchBoxContext context)
        {
            var list = new List<User>();

            // AleF
            list.Add(new User
            {
                UserName = "AleF",
                Email = "afederici75@gmail.com",
                FirstName = "Federico",
                MiddleName = "Alessandro",
                LastName = "Federici",
            });

            // Rechner
            list.Add(new User
            {
                UserName = "Rechner",
                Email = "me@ke4fox.net",
                FirstName = "Zachary",
                LastName = "Sturgeon",
            });

            // keith.center
            list.Add(new User
            {
                UserName = "keith.center",
                Email = "kcenter@gmail.com",
                FirstName = "Zachary",
                LastName = "Sturgeon",
            });

            // disabled.user
            list.Add(new User
            {
                UserName = "disabled.user",
                Email = "somewhere@someserver.com",
                FirstName = "John",
                LastName = "Smith",
                IsDisabled = true
            });

            return list;
        }

        #endregion

        static async Task<bool> CreateDefaultAdminIfNecessary(MatchBoxContext context)
        {
            var adminSearch = await context.TryFindUserByName(MatchBoxContext.AdminUserName);
            if (!adminSearch.HasResult)
            {
                var admin = GenerateDefaultAdmin();
                context.Users.Add(admin);
                
                return true;
            }

            return false;
        }
        
    }
}
