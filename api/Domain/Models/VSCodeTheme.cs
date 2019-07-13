using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace VSCThemesStore.WebApi.Domain.Models
{
    public class VSCodeTheme
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        public string Version { get; set; }

        public List<Theme> Themes { get; set; } = new List<Theme>();
    }
}
