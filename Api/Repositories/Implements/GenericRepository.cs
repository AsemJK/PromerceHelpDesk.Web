using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PromerceCRM.API.Repository.Interfaces;
using System.Linq.Expressions;

namespace PromerceCRM.API.Repository.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public Task<int> CountAsync()
        {
            return dbSet.CountAsync();
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public async Task<T> DeleteAsync(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);
            dbSet.Remove(entity);
            return entity;
        }

        public void DeleteRange(List<T> entityList)
        {
            dbSet.RemoveRange(entityList);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool disabledTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (disabledTracking)
                query = query.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            if (include != null)
                query = include(query);
            if (orderBy != null)
                return orderBy(query).ToList();
            else return query.ToList();

        }

        public T GetByIdAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool disabledTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (disabledTracking)
                query = query.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            if (include != null)
                query = include(query);
            return query.FirstOrDefault();
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
