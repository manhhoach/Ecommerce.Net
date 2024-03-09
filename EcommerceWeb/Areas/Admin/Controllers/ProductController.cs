
using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var data = _unitOfWork._ProductRepository.GetAll().ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork._ProductRepository.Add(data);
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
            var data = _unitOfWork._ProductRepository.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Product data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork._ProductRepository.Update(data);
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
            var data = _unitOfWork._ProductRepository.Get(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            _unitOfWork._ProductRepository.Delete(data);
            _unitOfWork.Save();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        //[HttpPost]
        //public IActionResult Delete(Product data)
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
