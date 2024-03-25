using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }

        public Book Book { get; set; } = default!;
    }
}
