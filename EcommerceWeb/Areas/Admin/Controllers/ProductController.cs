
using Ecommerce.DataAccess.IRepository;
using Ecommerce.Models.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var data = _unitOfWork._ProductRepository.GetAll().ToList();
            return View(data);
        }

        public IActionResult Upsert(int? id)
        {
            var product = new Product();
            if (id != 0 && id != null)
            {
                product = _unitOfWork._ProductRepository.Get(x => x.Id == id);
            }
            var Category = _unitOfWork._CategoryRepository.GetAll().Select(e => new SelectListItem()
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = product.CategoryId == e.Id
            }).ToList();

            ProductVM data = new ProductVM()
            {
                Product = product,
                Category = Category
            };


            return View(data);
        }

        string UploadAndReturnUrl(IFormFile file, string folderName)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string folderPath = Path.Combine(wwwRootPath, folderName);
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            using (var fileStream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return folderName + fileName;

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM data, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string imageUrl = UploadAndReturnUrl(file, "images/product/");
                    if (!string.IsNullOrEmpty(data.Product.ImageUrl))
                    {
                        // delete old image
                        string oldPath = Path.Combine(wwwRootPath, data.Product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    data.Product.ImageUrl = "/" + imageUrl;
                }
                if (data.Product.Id == 0)
                {
                    _unitOfWork._ProductRepository.Add(data.Product);
                }
                else
                {
                    _unitOfWork._ProductRepository.Update(data.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                data.Category = _unitOfWork._CategoryRepository.GetAll().Select(e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Id.ToString()
                }).ToList();

                return View(data);
            }

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

        #region API call
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _unitOfWork._ProductRepository.GetAll().ToList();
            return Json(new { data });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var data = _unitOfWork._ProductRepository.Get(x => x.Id == id);
            if (data == null)
            {
                return Json(new { success = false, message = "Data not found" });
            }

            string path = Path.Combine(_webHostEnvironment.WebRootPath, data.ImageUrl.TrimStart('/'));
            FileHelper.Delete(path);

            _unitOfWork._ProductRepository.Delete(data);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
