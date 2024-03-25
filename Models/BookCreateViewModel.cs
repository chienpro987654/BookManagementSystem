using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class BookCreateViewModel
    {
        public Book Book { get; set; } = default!;
        public List<Author>? Authors { get; set; } = default!;
        public List<Category>? Categories { get; set; } = default!;
        [Required]
        [DisplayName("Categories")]
        public List<int> CategoryIdList { get; set; } = new List<int>();
        public IFormFile? Image { get; set; } = default!;
        [DisplayName("Source File")]
        public IFormFile? File { get; set; } = default!;
    }
}
