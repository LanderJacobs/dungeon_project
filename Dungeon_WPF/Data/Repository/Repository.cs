using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.Data.Repository
{
    public class Repository<T>: IRepository<T> where T : class, new()
    {
        protected DbContext context { get; }

        public Repository(DbContext _context)
        {
            this.context = _context;
        }
        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
        }
    }
}
