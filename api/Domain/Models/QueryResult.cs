using System.Collections.Generic;

namespace VSCThemesStore.WebApi.Domain.Models
{
    public class QueryResult<T>
    {
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
