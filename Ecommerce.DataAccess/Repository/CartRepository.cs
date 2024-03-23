using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;

namespace Ecommerce.DataAccess.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext db) : base(db)
        {
        }
        public override void Update(Cart entity)
        {
            base.Update(entity);
        }
    }
}
