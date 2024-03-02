using EcommerceWeb.Data;
using EcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var data = _db.Categories.OrderBy(x => x.DisplayOrder).ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category data)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(data);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
    }
}
