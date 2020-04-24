using MatchBox.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Db
{
    // Inspired by https://stackoverflow.com/questions/18716928/how-to-write-a-async-method-with-out-parameter
    // See Jerry Nixon's options

    public static class MatchBoxContextUserExtensions
    {
        public static async Task<AsyncTryResult<User>> TryFindUserByName(this MatchBoxContext context, string userName)
        {
            return new AsyncTryResult<User>(
                await context.Users.SingleOrDefaultAsync(u => u.UserName==userName)
                );
        }
    }
}
