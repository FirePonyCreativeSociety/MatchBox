using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchBox.Contracts;
using MatchBox.Db;
using Microsoft.AspNetCore.Mvc;

namespace MatchBox.Controllers
{
    public class ThemeCampsController : RESTControllerBase<ThemeCamp>
    {
        public ThemeCampsController(MatchBoxContext context)
            : base(context)
        {

        }
    }
}