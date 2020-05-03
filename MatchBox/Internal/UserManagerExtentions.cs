using MatchBox.Data.Extensions;
using MatchBox.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Internal
{
    public static class UserManagerExtentions
    {
        public static async Task<AsyncTryFindResult<TUser>> FindUserByUsernameOrEmail<TUser>(this UserManager<TUser> instance, string usernameOrEmail)
            where TUser : class
        {
            var user = await instance.FindByNameAsync(usernameOrEmail);
            user = user ?? await instance.FindByEmailAsync(usernameOrEmail);

            return new AsyncTryFindResult<TUser>(user);
        }
    }
}
