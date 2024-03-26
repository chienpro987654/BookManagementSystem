namespace BookManagementSystem.Models
{
    public class BookReadViewModel
    {
        public Book Book { get; set; } = default!;
        public List<string> ArrContent { get; set; } = default!;
        public List<int> ArrIndexStart { get; set; } = default!;
        public int PageTotal { get; set; }
        public int PrevPage { get; set; } = -1;
        public int CurrentPage { get; set; }
        public int NextPage { get; set; } = -1;
    }
}
