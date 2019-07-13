namespace VSCThemesStore.WebApi.Domain.Models
{
    public class Statistics
    {
        public int InstallCount { get; set; }
        public int Downloads { get; set; }
        public double AverageRating { get; set; }
        public double WeightedRating { get; set; }
        public int RatingCount { get; set; }
        public double TrendingDaily { get; set; }
        public double TrendingWeekly { get; set; }
        public double TrendingMonthly { get; set; }
    }
}
