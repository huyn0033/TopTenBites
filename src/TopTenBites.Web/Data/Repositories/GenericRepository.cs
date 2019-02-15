using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;

namespace TopTenBites.Web.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>, IDisposable where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        
        public GenericRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public T Add(T entity) {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Update(T entity) {
            //_dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SetModified(entity);
            _dbContext.SaveChanges();
        }

        public void Remove(T entity) {
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Remove(Expression<Func<T, bool>> filter)
        {
            IEnumerable<T> objects = _dbContext.Set<T>().Where(filter).AsEnumerable();
            if (objects.Any())
            {
                foreach (T o in objects)
                {
                    _dbContext.Set<T>().Remove(o);
                }
                _dbContext.SaveChanges();
            }
        }

        public void RemoveById(int id)
        {
            T entity = GetById(id);
            if (entity != null)
            {
                _dbContext.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public T GetById(object id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public T Get(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                    string includeProperties = "")
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.ToList();
        }

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
