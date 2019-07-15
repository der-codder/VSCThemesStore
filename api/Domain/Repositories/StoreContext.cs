using VSCThemesStore.WebApi.Domain.Models;
using MongoDB.Driver;

namespace VSCThemesStore.WebApi.Domain.Repositories
{
    public interface IStoreContext
    {
        IMongoCollection<ExtensionMetadata> ExtensionsMetadata { get; }
        IMongoCollection<VSCodeTheme> Themes { get; }
    }

    public class StoreContext : IStoreContext
    {
        private readonly IMongoDatabase _db;

        public IMongoCollection<ExtensionMetadata> ExtensionsMetadata =>
            _db.GetCollection<ExtensionMetadata>("ExtensionsMetadata");

        public IMongoCollection<VSCodeTheme> Themes =>
            _db.GetCollection<VSCodeTheme>("Themes");

        public StoreContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            _db = client.GetDatabase(config.Database);

            SetupIndexes();
        }

        /// <summary>
        /// Sets up the database indexes for collections
        /// </summary>
        private void SetupIndexes()
        {
            var extensionMetadataBuilder = Builders<ExtensionMetadata>.IndexKeys;

            CreateIndexModel<ExtensionMetadata>[] indexModel = new[]
            {
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder.Descending(m => m.Statistics.Downloads)),
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder.Descending(m => m.LastUpdated)),
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder.Ascending(m => m.PublisherDisplayName)),
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder.Ascending(m => m.DisplayName)),
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder.Descending(m => m.Statistics.WeightedRating)),
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder.Descending(m => m.Statistics.TrendingWeekly)),
                new CreateIndexModel<ExtensionMetadata>(
                    extensionMetadataBuilder
                        .Text(m => m.PublisherDisplayName)
                        .Text(m => m.DisplayName)
                        .Text(m => m.Description)
                )
            };
            ExtensionsMetadata.Indexes.CreateMany(indexModel);
        }
    }
}
