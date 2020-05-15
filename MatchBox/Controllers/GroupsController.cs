using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Configuration;
using MatchBox.Data;
using MatchBox.Data.Models;
using MatchBox.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class GroupsController : RESTControllerBase<Group, DbGroup>
    {
        public GroupsController(SecurityConfiguration config, MatchBoxDbContext dbContext, IMapper mapper)
            : base(config, dbContext, mapper)
        {

        }

        protected override IQueryable<DbGroup> ControllerDbSet => DbContext.Groups;

        [HttpGet(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<ActionResult<IEnumerable<Group>>> Get(QueryModel model)
        {
            return base.GetAll(model);
        }
    }
}