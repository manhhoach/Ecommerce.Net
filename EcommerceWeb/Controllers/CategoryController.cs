
using Ecommerce.DataAccess.CategoryRepository;
using Ecommerce.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var data = _categoryRepository.GetAll().OrderBy(x => x.DisplayOrder).ToList();
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
                _categoryRepository.Add(data);
                _categoryRepository.Save();
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
            var data = _categoryRepository.Get(x => x.Id == id);
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
                _categoryRepository.Update(data);
                _categoryRepository.Save();
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
            var data = _categoryRepository.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            _categoryRepository.Delete(data);
            _categoryRepository.Save();
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
