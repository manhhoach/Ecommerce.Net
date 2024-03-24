using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommerce.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        private DbSet<T> _dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual T Get(Expression<Func<T, bool>>? filter, string? properties = null, bool isTracked = false)
        {
            IQueryable<T> query = _dbSet;
            if (!isTracked)
            {
                query = query.AsNoTracking(); // _dbSet.AsNoTracking().Where(filter).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(properties))
            {
                foreach (string prop in properties.Split(","))
                {
                    query = query.Include(prop);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.FirstOrDefault();
        }

        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? properties = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(properties))
            {
                foreach (string prop in properties.Split(","))
                {
                    query = query.Include(prop);
                }
            }
            return query.ToList();
        }

        public virtual void Update(T entity)
        {
            _db.Update(entity);
        }
    }
}
