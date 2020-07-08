namespace ProductsMicroService.Data.Utilities
{
    public interface IQueryParameters
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}