namespace KeyStone.Web.Common.Models
{
    public class PagedResponse<T>
    {
        public List<T> Page { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}