namespace BookManagementSystem.Services
{
    public interface ICartService
    {
        Task<int> CreateCartForUser();
    }
}
