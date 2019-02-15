using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveById(int id);
        void Remove(Expression<Func<T, bool>> filter);
        T GetById(object id);
        T Get(Expression<Func<T, bool>> filter, string includeProperties = "");
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
    }
}
