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
        public RESTControllerBase(MatchBoxDbContext context)
            : base()
        {
            Context = context;
        }

        public MatchBoxDbContext Context { get; }

        protected async Task<T> FindById(int id)
        {
            return await Context.FindAsync(typeof(T), id) as T;            
        }

        [HttpPost()]
        public async Task<ActionResult<T>> Create(T value)
        {
            Context.Add(value);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<T>> Delete(int id)
        {
            var tmp = await FindById(id);

            if (tmp != null)
                return NotFound();

            Context.Remove(tmp);
            await Context.SaveChangesAsync();

            return tmp;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> GetById(int id)
        {
            var tmp = await FindById(id);

            if (tmp != null) 
                return tmp; 
            else 
                return NotFound();
        }        
    }
}