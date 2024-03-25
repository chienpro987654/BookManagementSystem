using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = default!;
        [Required]
        public string Description { get; set; } = default!;
        public string ImageName { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        [DisplayName("Author")]
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public long Price { get; set; }
        [Display(Name = "Sold")]
        public int SoldQuantity { get; set; }
        [Range(0, 5)]
        public double Rating { get; set; } = 0;
        public int Status { get; set; }

        [DefaultValue(0)]
        public int Discount { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [DisplayName("Categories")]
        public List<Category>? Categories { get; set; } = default!;
        public List<BookCategory>? BookCategories { get; set; } = default!;
        public Author? Author { get; set; }

        public static IEnumerable<SelectListItem> GetStatusList()
        {
            yield return new SelectListItem { Value = "0", Text = "Available", Selected = true };
            yield return new SelectListItem { Value = "1", Text = "Sold Out" };
        }
    }
}
