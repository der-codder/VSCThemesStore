using System.Collections.Generic;

namespace VSCThemesStore.WebApi.Controllers.Resources
{
    public class ExtensionResource : ExtensionMetadataResource
    {
        public List<ThemeResource> Themes { get; set; }
    }
}
