using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data;
using MatchBox.Data.Models;
using System.Linq;

namespace MatchBox.Controllers
{
    public class GroupsController : RESTControllerBase<Group, DbGroup>
    {
        public GroupsController(MatchBoxDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {

        }

        protected override IQueryable<DbGroup> ControllerDbSet => DbContext.Groups;
    }
}