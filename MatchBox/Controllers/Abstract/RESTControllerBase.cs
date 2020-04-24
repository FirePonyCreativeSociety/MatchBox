using MatchBox.Contracts;
using MatchBox.Db;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    // Inspired by https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio

    public class RESTControllerBase<T> : MatchBoxControllerBase
        where T : EntityBase, new()
    {
        public RESTControllerBase(MatchBoxContext context)
            : base()
        {
            Context = context;
        }

        public MatchBoxContext Context { get; }

        [HttpPost()]
        public async Task<ActionResult<T>> Create(T value)
        {
            Context.Add(value);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = value.Id}, value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<T>> Update(long id, T value)
        {
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(long id)
        {
            return NotFound();
        }

        
    }
}