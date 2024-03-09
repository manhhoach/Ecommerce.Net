namespace Ecommerce.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository _CategoryRepository { get; }
        IProductRepository _ProductRepository { get; }
        void Save();
    }
}
