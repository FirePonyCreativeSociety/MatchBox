using MatchBox.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    public class RESTControllerBase<T> : MatchBoxControllerBase
        where T : class, new()
    {
        public RESTControllerBase(MatchBoxContext context)
            : base()
        {
            Context = context;
        }

        public MatchBoxContext Context { get; }

        [HttpPost()]
        public async Task Create(T value)
        { 
        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(long id)
        {
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task Update(long id, T value)
        { 
        
        }
    }
}