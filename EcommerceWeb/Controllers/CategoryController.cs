using EcommerceWeb.Data;
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
    }
}
