using BookManagementSystem.Data;
using BookManagementSystem.Models;

namespace BookManagementSystem.Services
{
    public class CartService : ICartService
    {
        ApplicationDbContext _db;

        public CartService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> CreateCartForUser()
        {
            Cart cart = new Cart();
            _db.Carts.Add(cart);
            await _db.SaveChangesAsync();
            return cart.Id;
        }
    }
}
