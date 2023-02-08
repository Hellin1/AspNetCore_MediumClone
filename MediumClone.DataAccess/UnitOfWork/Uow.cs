using MediumClone.DataAccess.Contexts;
using MediumClone.DataAccess.Interfaces;
using MediumClone.DataAccess.Repositories;
using MediumClone.Entities.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumClone.DataAccess.UnitOfWork
{
    public class Uow : IUow
    {
        private readonly NlogContext _context;

        public Uow(NlogContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_context);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
