using VSCThemesStore.WebApi.Services;
using FluentScheduler;

namespace VSCThemesStore.WebApi.ScheduledJobs
{
    public class RefreshGalleryRegistry : Registry
    {
        public RefreshGalleryRegistry(IGalleryRefreshService refreshingService)
        {
            Schedule(async () => await refreshingService.RefreshGallery())
                .ToRunEvery(60).Minutes();
            // .ToRunNow().AndEvery(60).Minutes();
        }
    }
}
