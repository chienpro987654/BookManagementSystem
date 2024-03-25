namespace BookManagementSystem.Models
{
    public class HomeSearchViewModel
    {
        public string sQuery { get; set; } = string.Empty;
        public List<Book>? BookResults { get; set; }
        public List<Category>? Categories { get; set; }
        public List<int>? CategoryIdList { get; set; }
    }
}
