namespace SchoolManagement.API.Models
{
    public class PagedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchQuery { get; set; }
        public string? UserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}