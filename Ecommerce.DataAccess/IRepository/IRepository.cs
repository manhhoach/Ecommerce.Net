using System.Linq.Expressions;

namespace Ecommerce.DataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? properties = null);
        T Get(Expression<Func<T, bool>>? filter = null, string? properties = null, bool isTracked = false);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
