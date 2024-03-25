using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = "Unknown";
        public string Description { get; set; } = "N/A";
        public List<Book>? Books { get; set; }
        public List<Category>? Categories { get; set; }
        public List<AuthorCategory>? AuthorCategories { get; set; }
    }
}