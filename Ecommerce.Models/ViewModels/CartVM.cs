using Ecommerce.Models.Models;

namespace Ecommerce.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> Carts { get; set; }
        public double OrderTotal { get; set; }
    }
}
