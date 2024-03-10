using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public override IEnumerable<Product> GetAll()
        {
            return _db.Products.Include(x => x.Category).Select(e => new Product()
            {
                Author = e.Author,
                Title = e.Title,
                Id = e.Id,
                Category = new Category() { Name = e.Category.Name },
            }).ToList();
        }
    }
}
