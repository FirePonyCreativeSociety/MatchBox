using AutoMapper;
using MatchBox.API.Model;
using MatchBox.Data;
using MatchBox.Data.Model;
using System.Linq;

namespace MatchBox.Controllers
{
    public class GroupsController : RESTControllerBase<Group, DbGroup>
    {
        public GroupsController(MatchBoxDbContext context, IMapper mapper)
            : base(context, mapper)
        {

        }

        protected override IQueryable<DbGroup> ControllerDbSet => Context.Groups;
    }
}