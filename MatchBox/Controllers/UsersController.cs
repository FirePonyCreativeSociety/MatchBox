using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchBox.Contracts;
using MatchBox.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;

namespace MatchBox.Controllers
{
    public class UsersController : RESTControllerBase<User>
    {
        public UsersController(MatchBoxDbContext context)
            : base(context)
        {

        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<User>>> GetAll(int skip, int take, string orderBy)
        {
            IQueryable<User> tmp = Context.Users;

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = nameof(EntityBase.Id);

            tmp = tmp.OrderBy(orderBy);

            if (skip > 0)
                tmp = tmp.Skip(skip);

            if (take > 0)
                tmp = tmp.Take(take);
            
            return await tmp.ToListAsync();
        }
    }
}