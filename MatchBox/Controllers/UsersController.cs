using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatchBox.Contracts;
using MatchBox.Db;
using Microsoft.AspNetCore.Mvc;

namespace MatchBox.Controllers
{
    public class UsersController : RESTControllerBase<User>
    {
        public UsersController(MatchBoxContext context)
            : base(context)
        {

        }
    }
}