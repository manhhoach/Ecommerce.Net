using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(20)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Display Order must be greater than 0")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
