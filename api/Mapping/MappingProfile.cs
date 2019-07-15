using AutoMapper;
using VSCThemesStore.WebApi.Domain.Models;
using VSCThemesStore.WebApi.Controllers.Resources;

namespace VSCThemesStore.WebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<ExtensionMetadata, ExtensionResource>()
                .IncludeBase<ExtensionMetadata, ExtensionMetadataResource>()
                .ForMember(extMeta => extMeta.Themes, opt => opt.Ignore());
            CreateMap<ExtensionMetadata, ExtensionMetadataResource>();
            CreateMap<Statistics, StatisticResource>();
            CreateMap<Theme, ThemeResource>()
                .ForMember(dest => dest.Colors, opt => opt.Ignore())
                .ForMember(dest => dest.TokenColors, opt => opt.Ignore());
            CreateMap<TokenColorSettings, TokenColorSettingsResource>();
            CreateMap<TokenColor, TokenColorResource>();
            CreateMap<TokenColorSettings, TokenColorSettingsResource>();
            CreateMap<TokenColor, TokenColorResource>();
            CreateMap(typeof(QueryResult<>), typeof(QueryResultResource<>));

            // API Resource to Domain
            CreateMap<StoreQueryResource, StoreQuery>()
                .AfterMap((_, q) => q.NormalizeQueryParams());
        }
    }
}
