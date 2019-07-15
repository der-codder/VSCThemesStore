using System.Threading.Tasks;
using AutoMapper;
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
    public class MetadataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExtensionsMetadataRepository _metadataRepository;

        public MetadataController(
            IMapper mapper,
            IExtensionsMetadataRepository metadataRepository)
        {
            _mapper = mapper;
            _metadataRepository = metadataRepository;
        }

        // GET api/metadata?pageNumber=2&pageSize=10&sortBy=Downloads
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<QueryResultResource<ExtensionMetadataResource>> Index(
            [FromQuery] StoreQueryResource queryResource)
        {
            var query = _mapper.Map<StoreQueryResource, StoreQuery>(queryResource);

            var queryResult = await _metadataRepository.GetItems(query);

            return _mapper.Map<QueryResult<ExtensionMetadata>, QueryResultResource<ExtensionMetadataResource>>(queryResult);
        }

        // GET api/metadata/id
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExtensionMetadataResource>> Get(string id)
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

            return _mapper.Map<ExtensionMetadata, ExtensionMetadataResource>(metadata);
        }
    }
}
