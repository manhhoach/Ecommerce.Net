using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [BindProperty]
        private CartVM cartVM { get; set; }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cartVM = new()
            {
                Carts = _unitOfWork._CartRepository.GetAll(x => x.UserId == userId, "Product"),
                OrderHeader = new()
            };
            foreach (var cart in cartVM.Carts)
            {
                cart.Price = GetPriceFromQuantity(cart);
                cartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(cartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork._AppUserRepository.Get(x => x.Id == userId);
            if (user != null)
            {
                CartVM cartVM = new()
                {
                    Carts = _unitOfWork._CartRepository.GetAll(x => x.UserId == userId, "Product"),
                    OrderHeader = new()
                    {
                        User = user,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        State = user.State,
                        StreetAddress = user.StreetAddress,
                        City = user.City,
                        PostalCode = user.PostalCode
                    }
                };

                foreach (var cart in cartVM.Carts)
                {
                    cart.Price = GetPriceFromQuantity(cart);
                    cartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
                }
                return View(cartVM);
            }
            return View("NotFound");

        }

        [HttpPost]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork._AppUserRepository.Get(x => x.Id == userId);
            if (user != null)
            {
                cartVM.Carts = _unitOfWork._CartRepository.GetAll(x => x.UserId == userId, "Product");
                cartVM.OrderHeader.User = user;
                cartVM.OrderHeader.OrderDate = DateTime.Now;
                cartVM.OrderHeader.UserId = userId;


                foreach (var cart in cartVM.Carts)
                {
                    cart.Price = GetPriceFromQuantity(cart);
                    cartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
                }
                if (cartVM.OrderHeader.User.CompanyId.GetValueOrDefault() == 0)
                {
                    cartVM.OrderHeader.PaymentStatus = OrderStatus.PaymentStatusPending;
                    cartVM.OrderHeader.OrderStatus = OrderStatus.StatusPending;
                }
                else
                {
                    cartVM.OrderHeader.PaymentStatus = OrderStatus.PaymentStatusDelayedPayment;
                    cartVM.OrderHeader.OrderStatus = OrderStatus.StatusApproved;
                }
                _unitOfWork._OrderHeaderRepository.Add(cartVM.OrderHeader);
                _unitOfWork.Save();

                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var item in cartVM.Carts)
                {
                    orderDetails.Add(new OrderDetail()
                    {
                        ProductId = item.ProductId,
                        OrderHeaderId = cartVM.OrderHeader.Id,
                        Price = item.Price,
                        Count = item.Count,
                    });
                }
                _unitOfWork._OrderDetailRepository.AddRange(orderDetails);
                _unitOfWork.Save();

                return View(cartVM);
            }
            return View("NotFound");

        }

        private static double GetPriceFromQuantity(Cart cart)
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
            if (data != null)
            {
                if (data.Count <= 1)
                {
                    _unitOfWork._CartRepository.Delete(data);
                }
                else
                {
                    data.Count--;
                    _unitOfWork._CartRepository.Update(data);
                }
                _unitOfWork.Save();
            }
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
