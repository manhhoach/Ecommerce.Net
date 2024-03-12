using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public int Name { get; set; }

        public string? State { get; set; }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
    }
}
