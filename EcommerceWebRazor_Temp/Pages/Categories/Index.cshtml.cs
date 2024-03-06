using EcommerceWebRazor_Temp.Data;
using EcommerceWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceWebRazor_Temp.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public List<Category> CategoryList { get; set; }

        public IndexModel(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }
        public void OnGet()
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
