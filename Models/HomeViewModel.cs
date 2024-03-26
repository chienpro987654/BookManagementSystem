namespace BookManagementSystem.Models
{
    public class HomeViewModel
    {
        public List<Book>? BookRecommend { get; set; } = new List<Book>();
        public List<Book>? BookTopSale { get; set; } = new List<Book>();
        public List<Book>? BooksRecent { get; set; } = new List<Book>();
        public List<Book>? BooksFromCategory { get; set; } = new List<Book>();
        public List<Category>? Categories { get; set; } = new List<Category>();
        public int CategoryIdSelected { get; set; }
        public string SortOrderSelected { get; set; } = string.Empty;

        public static Dictionary<string, string> getSortOrderList()
        {
            Dictionary<string, string> dictOrder = new Dictionary<string, string>();
            dictOrder.Add("default", "Default");
            dictOrder.Add("name_desc", "Name Z - A");
            dictOrder.Add("name_asc", "Name A - Z");
            dictOrder.Add("upload_desc", "Lastest");
            dictOrder.Add("upload_asc", "Oldest");
            dictOrder.Add("publish_desc", "Published Lastest");
            dictOrder.Add("publish_asc", "Published Oldest");
            return dictOrder;
        }
    }
}
