namespace Ecommerce.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository _CategoryRepository { get; }
        void Save();
    }
}
