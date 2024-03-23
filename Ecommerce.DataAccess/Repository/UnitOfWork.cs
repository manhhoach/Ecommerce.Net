using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.IRepository;

namespace Ecommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository _CategoryRepository { get; private set; }
        public IProductRepository _ProductRepository { get; private set; }
        public ICompanyRepository _CompanyRepository { get; private set; }
        public ICartRepository _CartRepository { get; private set; }
        public IAppUserRepository _AppUserRepository { get; private set; }

        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            _CategoryRepository = new CategoryRepository(_db);
            _ProductRepository = new ProductRepository(_db);
            _CompanyRepository = new CompanyRepository(_db);
            _CartRepository = new CartRepository(_db);
            _AppUserRepository = new AppUserRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
