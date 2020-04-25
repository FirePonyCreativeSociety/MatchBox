using AutoMapper;
using MatchBox.API.Model;
using MatchBox.Data;
using MatchBox.Data.Model;
using System.Linq;

namespace MatchBox.Controllers
{
    public class EventsController : RESTControllerBase<Event, DbEvent>
    {
        public EventsController(MatchBoxDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {

        }

        protected override IQueryable<DbEvent> ControllerDbSet => DbContext.Events;
    }
}