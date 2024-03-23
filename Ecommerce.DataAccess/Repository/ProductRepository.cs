using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                ImageUrl = e.ImageUrl,
                ListPrice = e.ListPrice,
                Price100 = e.Price100,
                Category = new Category() { Name = e.Category.Name },
            }).ToList();
        }

        public override Product Get(Expression<Func<Product, bool>> filter)
        {
            return _db.Products.Where(filter).Include(x => x.Category).Select(e => new Product()
            {
                Author = e.Author,
                Title = e.Title,
                Id = e.Id,
                ImageUrl = e.ImageUrl,
                ListPrice = e.ListPrice,
                Price100 = e.Price100,
                Description = e.Description,
                CategoryId = e.CategoryId,
                ISBN = e.ISBN,
                Price = e.Price,
                Price50 = e.Price50,
                Category = new Category() { Name = e.Category.Name },
            }).FirstOrDefault();
        }
    }
}
