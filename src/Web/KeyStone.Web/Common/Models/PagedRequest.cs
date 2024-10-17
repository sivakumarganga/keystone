namespace KeyStone.Web.Common.Models
{
    public class PagedRequest
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int PageCount { get; set; }
        public string Keyword { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
    }
}