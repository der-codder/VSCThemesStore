using System;
using System.Collections.Generic;
using VSCThemesStore.WebApi.Domain.Models;

namespace VSCThemesStore.WebApi.Services
{
    public class ExtensionQueryResponseMetadata
    {
        public int RequestResultTotalCount { get; set; }
        public List<ExtensionMetadata> Items { get; }

        public ExtensionQueryResponseMetadata()
        {
            Items = new List<ExtensionMetadata>();
        }
    }
}
