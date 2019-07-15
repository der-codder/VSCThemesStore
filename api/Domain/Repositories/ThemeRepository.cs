using System.Threading.Tasks;
using VSCThemesStore.WebApi.Domain.Models;
using MongoDB.Driver;

namespace VSCThemesStore.WebApi.Domain.Repositories
{
    public interface IThemeRepository
    {
        Task<VSCodeTheme> GetTheme(string id);
        Task Create(VSCodeTheme theme);
        Task<bool> Update(VSCodeTheme theme);
    }

    public class ThemeRepository : IThemeRepository
    {
        private readonly IStoreContext _context;

        public ThemeRepository(IStoreContext context) => _context = context;

        public Task<VSCodeTheme> GetTheme(string id)
        {
            var filter = Builders<VSCodeTheme>.Filter.Eq(m => m.Id, id);

            return _context.Themes
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task Create(VSCodeTheme theme) =>
            await _context.Themes.InsertOneAsync(theme);

        public async Task<bool> Update(VSCodeTheme theme)
        {
            var updateResult = await _context.Themes
                .ReplaceOneAsync(
                    filter: g => g.Id == theme.Id,
                    replacement: theme
                );

            return updateResult.IsAcknowledged
                && updateResult.ModifiedCount > 0;
        }
    }
}
