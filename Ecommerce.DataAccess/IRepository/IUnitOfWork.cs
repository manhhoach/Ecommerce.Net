namespace Ecommerce.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository _CategoryRepository { get; }
        IProductRepository _ProductRepository { get; }
        ICompanyRepository _CompanyRepository { get; }
        ICartRepository _CartRepository { get; }
        IAppUserRepository _AppUserRepository { get; }
        IOrderDetailRepository _OrderDetailRepository { get; }
        IOrderHeaderRepository _OrderHeaderRepository { get; }
        void Save();
    }
}
