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
        public IActionResult Create(Category data)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(data);
                _db.SaveChanges();
                TempData["success"] = "Created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var data = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Category data)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(data);
                _db.SaveChanges();
                TempData["success"] = "Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var data = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(data);
            _db.SaveChanges();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public IActionResult Delete(Category data)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Categories.Remove(data);
        //        _db.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}
    }
}
