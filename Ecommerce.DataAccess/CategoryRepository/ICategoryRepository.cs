using Ecommerce.DataAccess.Repository;
using Ecommerce.Models.Models;

namespace Ecommerce.DataAccess.CategoryRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Save();
    }
}
