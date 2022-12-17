using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.Data.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T Get(params Expression<Func<T, bool>>[] requirements);
        IEnumerable<T> GetAllWithRequirements(params Expression<Func<T, bool>>[] requirements);
    }
}
