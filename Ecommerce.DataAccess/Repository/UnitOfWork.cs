using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.IRepository;

namespace Ecommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository _CategoryRepository { get; private set; }
        public IProductRepository _ProductRepository { get; private set; }

        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            _CategoryRepository = new CategoryRepository(_db);
            _ProductRepository = new ProductRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
