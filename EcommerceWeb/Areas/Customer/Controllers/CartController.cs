using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            CartVM cartVM = new CartVM()
            {
                Carts = _unitOfWork._CartRepository.GetAll(x => x.UserId == userId, "Product"),
            };
            foreach (var cart in cartVM.Carts)
            {
                cart.Price = GetPriceFromQuantity(cart);
                cartVM.OrderTotal += cart.Price * cart.Count;
            }
            return View(cartVM);
        }

        public IActionResult Summary()
        {
            return View();
        }

        private double GetPriceFromQuantity(Cart cart)
        {
            if (cart.Count <= 50)
            {
                return cart.Product.Price;
            }
            else if (cart.Count <= 100)
            {
                return cart.Product.Price50;
            }
            else
            {
                return cart.Product.Price100;
            }
        }

        public IActionResult Plus(int id)
        {
            var data = _unitOfWork._CartRepository.Get(x => x.Id == id);
            if (data != null)
            {
                data.Count++;
                _unitOfWork._CartRepository.Update(data);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            var data = _unitOfWork._CartRepository.Get(x => x.Id == id);
            if (data != null && data.Count <= 1)
            {
                _unitOfWork._CartRepository.Delete(data);
            }
            else
            {
                data.Count--;
                _unitOfWork._CartRepository.Update(data);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var data = _unitOfWork._CartRepository.Get(x => x.Id == id);
            _unitOfWork._CartRepository.Delete(data);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
