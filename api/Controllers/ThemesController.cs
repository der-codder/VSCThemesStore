using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using VSCThemesStore.WebApi.Controllers.Resources;
using VSCThemesStore.WebApi.Domain.Models;
using VSCThemesStore.WebApi.Domain.Repositories;

namespace VSCThemesStore.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExtensionsMetadataRepository _metadataRepository;
        private readonly IThemeRepository _themeRepository;

        public ThemesController(
            IMapper mapper,
            IExtensionsMetadataRepository metadataRepository,
            IThemeRepository themeStoreRepository)
        {
            _mapper = mapper;
            _metadataRepository = metadataRepository;
            _themeRepository = themeStoreRepository;
        }

        // GET api/themes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<ExtensionResource>> Get(string id)
        {
            var metadata = await _metadataRepository.GetExtensionMetadata(id);
            if (metadata == null)
            {
                Log.Information($"Couldn't find ExtensionMetadata '{id}'.");
                return NotFound(id);
            }
            if (metadata.Type != ExtensionType.Default)
            {
                Log.Information($"ExtensionMetadata '{id}' doesn't contain themes (ExtensionType: {metadata.Type})");
                return NotFound(id);
            }

            var theme = await _themeRepository.GetTheme(id);
            if (theme == null)
            {
                Log.Error($"Stored theme for '{id}' extension is empty.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreateExtensionResource(metadata, theme.Themes);
        }

        private ExtensionResource CreateExtensionResource(
            ExtensionMetadata metadata,
            IEnumerable<Theme> storedThemes)
        {
            var result = _mapper.Map<ExtensionMetadata, ExtensionResource>(metadata);
            result.Themes = storedThemes
                .Select(theme =>
                {
                    var themeResource = _mapper.Map<Theme, ThemeResource>(theme);
                    themeResource.TokenColors = theme.TokenColors
                        .Select(tc => _mapper.Map<TokenColor, TokenColorResource>(tc))
                        .ToList();
                    themeResource.Colors = new Dictionary<string, string>(
                        theme.Colors
                        .FindAll(c => !string.IsNullOrWhiteSpace(c.Value))
                        .Select(c => KeyValuePair.Create(c.PropertyName, c.Value)));

                    return themeResource;
                })
                .ToList();

            return result;
        }
    }
}
