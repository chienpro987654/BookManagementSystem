using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public List<CartItem>? CardItems { get; set; } = default!;
    }
}
