using MediumClone.DataAccess.Contexts;
using MediumClone.DataAccess.Interfaces;
using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly NlogContext _context;

        public Repository(NlogContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByFilter(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            return asNoTracking ? await _context.Set<T>().SingleOrDefaultAsync(filter) : await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(filter);
        }

        //public async Task<T> GetByFilter 

        public async Task<T> GetById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity, T unchanged)
        {
            //_context.Set<T>().Update(entity);
            _context.Entry(unchanged).CurrentValues.SetValues(entity);
        }
        

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>().AsQueryable();
        }

        public async Task<List<Blog>> GetRelationalData()
        {
            return await _context.Blogs.Include(x => x.BlogCategories).ThenInclude(x => x.Category).AsNoTracking().ToListAsync();
        }

        public async Task<Blog> GetRelationalDataById(Expression<Func<Blog, bool>> filter)
        {
            return await _context.Blogs.Include(x => x.Comments).ThenInclude(x => x.AppUser).AsNoTracking().SingleOrDefaultAsync(filter);
        }
    }
}
