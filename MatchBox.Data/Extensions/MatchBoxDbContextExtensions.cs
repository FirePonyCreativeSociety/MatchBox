using MatchBox.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MatchBox.Data.Extensions
{
    // Inspired by https://stackoverflow.com/questions/18716928/how-to-write-a-async-method-with-out-parameter
    // See Jerry Nixon's options

    public static class MatchBoxDbContextExtensions
    {
        public static async Task<AsyncTryFindResult<DbUser>> TryFindUserByName(this MatchBoxDbContext context, string userName)
        {
            return new AsyncTryFindResult<DbUser>(
                await context.Users.SingleOrDefaultAsync(u => u.UserName==userName)
                );
        }
    }
}