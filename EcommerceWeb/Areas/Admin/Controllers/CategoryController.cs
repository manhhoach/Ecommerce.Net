
using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleConstants.Admin)]
    public class CategoryController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IActionResult Index()
        {
            var data = _unitOfWork._CategoryRepository.GetAll().OrderBy(x => x.DisplayOrder).ToList();
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
                _unitOfWork._CategoryRepository.Add(data);
                _unitOfWork.Save();
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
            var data = _unitOfWork._CategoryRepository.Get(x => x.Id == id);
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
                _unitOfWork._CategoryRepository.Update(data);
                _unitOfWork.Save();
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
            var data = _unitOfWork._CategoryRepository.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            _unitOfWork._CategoryRepository.Delete(data);
            _unitOfWork.Save();
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
