using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EcommerceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork._ProductRepository.GetAll().ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            Product product = _unitOfWork._ProductRepository.Get(x => x.Id == id);
            if (product != null)
            {
                Cart cart = new Cart()
                {
                    Product = product,
                    Count = 1,
                    ProductId = id
                };
                return View(cart);
            }
            return View("NotFound");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var existedCart = _unitOfWork._CartRepository.Get(x => x.UserId == userId && x.ProductId == cart.ProductId);
            if (existedCart != null)
            {
                existedCart.Count += cart.Count;
                _unitOfWork._CartRepository.Update(existedCart);
            }
            else
            {
                // phải map sang obj khác nếu không bị lỗi: Cannot insert explicit value for identity column in table 'Carts' when IDENTITY_INSERT is set to OFF.
                var data = new Cart()
                {
                    Count = cart.Count,
                    UserId = userId,
                    ProductId = cart.ProductId
                };
                _unitOfWork._CartRepository.Add(data);

            }
            _unitOfWork.Save();
            TempData["success"] = "Add to cart successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
