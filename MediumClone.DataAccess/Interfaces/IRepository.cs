using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetById(object id);

        Task<T> GetByFilter(Expression<Func<T, bool>> filter, bool asNoTracking = false);

        Task CreateAsync(T entity);

        void Update(T entity, T unchanged);

        void Remove(T entity);

        IQueryable<T> GetQuery();

        Task<List<Blog>> GetRelationalData();

        Task<Blog> GetRelationalDataById(Expression<Func<Blog, bool>> filter);
    }
}
