using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class AuthorCreateViewModel
    {
        public Author Author { get; set; } = default!;
        public List<Category>? Categories { get; set; } = default!;
        [Required]
        public List<int> CategoryIdList { get; set; } = new List<int>();
    }
}
