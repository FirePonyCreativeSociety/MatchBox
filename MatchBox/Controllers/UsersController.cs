using AutoMapper;
using MatchBox.API.Model;
using MatchBox.Data;
using MatchBox.Data.Model;
using System.Linq;

namespace MatchBox.Controllers
{
    public class UsersController : RESTControllerBase<User, DbUser>
    {
        public UsersController(MatchBoxDbContext context, IMapper mapper)
            : base(context, mapper)
        {

        }

        protected override IQueryable<DbUser> ControllerDbSet => Context.Users;

        
    }
}