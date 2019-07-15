namespace VSCThemesStore.WebApi.Controllers.Resources
{
    public class StoreQueryResource
    {
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
