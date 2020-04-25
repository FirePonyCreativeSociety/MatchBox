using AutoMapper;
using MatchBox.API.Model;
using MatchBox.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MatchBox.Controllers
{
    // Inspired by https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio

    public abstract class RESTControllerBase<APIMODEL, DBMODEL> : MatchBoxControllerBase
        where APIMODEL : EntityBase, new()
        where DBMODEL : class, new()
    {
        public RESTControllerBase(MatchBoxDbContext context, IMapper mapper)
            : base()
        {
            Context = context;
            Mapper = mapper;
        }

        protected MatchBoxDbContext Context { get; }
        protected IMapper Mapper { get; }

        protected abstract IQueryable<DBMODEL> ControllerDbSet { get; }

        protected async Task<APIMODEL> FindById(int id)
        {
            return await Context.FindAsync(typeof(APIMODEL), id) as APIMODEL;            
        }

        [HttpPost()]
        public async Task<ActionResult<APIMODEL>> Create(APIMODEL value)
        {
            Context.Add(value);
            await Context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIMODEL>> Delete(int id)
        {
            var tmp = await FindById(id);

            if (tmp == null)
                return NotFound();

            Context.Remove(tmp);
            await Context.SaveChangesAsync();

            return tmp;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<APIMODEL>> GetById(int id)
        {
            var tmp = await FindById(id);

            if (tmp != null) 
                return tmp; 
            else 
                return NotFound();
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<APIMODEL>>> GetAll(int skip, int take, string orderBy)
        {
            // https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
            IQueryable<DBMODEL> dbSet = ControllerDbSet;

            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = nameof(EntityBase.Id);

            dbSet = dbSet.OrderBy(orderBy);

            if (skip > 0)
                dbSet = dbSet.Skip(skip);

            if (take > 0)
                dbSet = dbSet.Take(take);

            var dbData = await dbSet.ToArrayAsync();
            var result = Mapper.Map<APIMODEL[]>(dbData); // For some reason I need to use an array, not IEnumerable.
            return result;
        }
    }
}