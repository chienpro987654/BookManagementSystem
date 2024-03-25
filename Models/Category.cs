using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BookManagementSystem.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [AllowNull]
        public string? Description { get; set; } = string.Empty;

        public List<Book>? Books { get; set; } = default!;
        public List<BookCategory>? BookCategories { get; set; } = default!;

        public List<Author>? Authors { get; set; } = default!;
        public List<AuthorCategory>? AuthorCategories { get; set; }
    }
}