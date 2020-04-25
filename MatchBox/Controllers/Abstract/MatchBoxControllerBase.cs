using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public abstract class MatchBoxControllerBase : ControllerBase
    {
        public MatchBoxControllerBase()
            : base()
        {

        }
    }
}
