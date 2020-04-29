using AutoMapper;
using MatchBox.API.Models;
using MatchBox.Data;
using MatchBox.Models;
using MatchBox.Models.Interfaces;
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
        where APIMODEL : class, IIntId, new()
        where DBMODEL : class, new()
    {
        public RESTControllerBase(MatchBoxDbContext dbContext, IMapper mapper)
            : base()
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        protected MatchBoxDbContext DbContext { get; }
        protected IMapper Mapper { get; }

        protected abstract IQueryable<DBMODEL> ControllerDbSet { get; }

        protected async Task<APIMODEL> FindById(int id)
        {
            return await DbContext.FindAsync(typeof(APIMODEL), id) as APIMODEL;            
        }

        [HttpPost()]
        public async Task<ActionResult<APIMODEL>> Create(APIMODEL value)
        {
            var dbValue = Mapper.Map<DBMODEL>(value);
            DbContext.Add(dbValue);

            await DbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = value.GetId() }, value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIMODEL>> Delete(int id)
        {
            var tmp = await FindById(id);

            if (tmp == null)
                return NotFound();

            DbContext.Remove(tmp);
            await DbContext.SaveChangesAsync();

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
        public async Task<ActionResult<IEnumerable<APIMODEL>>> GetAll(QueryModel model)
        {
            // https://github.com/StefH/System.Linq.Dynamic.Core/wiki/Dynamic-Expressions
            IQueryable<DBMODEL> dbSet = ControllerDbSet;

            if (model.SortBy != null)
            {                
                var ordered = dbSet.OrderBy(model.SortBy.First());
                foreach (var f in model.SortBy.Skip(1))
                    ordered = ordered.ThenBy(f);

                dbSet = ordered;
            }
            else
            {
                //dbSet = dbSet.OrderBy(x => x.);
            }

            if (model.Include != null)
            {
                foreach (var i in model.Include)
                    dbSet = dbSet.Include(i);
            }
            
            if ((model.Skip.HasValue) && (model.Skip > 0))
                dbSet = dbSet.Skip(model.Skip.Value);

            if ((model.Take.HasValue) && (model.Take > 0))
                dbSet = dbSet.Take(model.Take.Value);

            var dbData = await dbSet.ToArrayAsync();
            var result = Mapper.Map<APIMODEL[]>(dbData); // For some reason I need to use an array, not IEnumerable.
            return result;
        }
    }
}