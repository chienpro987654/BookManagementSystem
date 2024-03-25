namespace BookManagementSystem.Models
{
    public class HomeViewModel
    {
        public List<Book>? BookRecommend { get; set; } = new List<Book>();
        public List<Book>? BookTopSale { get; set; } = new List<Book>();
        public List<Book>? BooksRecent { get; set; } = new List<Book>();
        public List<Book>? BooksFromCategory { get; set; } = new List<Book>();
        public List<Category>? Categories { get; set; } = new List<Category>();
    }
}
