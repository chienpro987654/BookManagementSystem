namespace BookManagementSystem.Models
{
    public class AuthorCategory
    {
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public Author Author { get; set; } = default!;
        public Category Category { get; set; } = default!;

    }
}
