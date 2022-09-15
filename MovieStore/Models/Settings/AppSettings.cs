namespace MovieStore.Models.Settings
{
    public sealed class AppSettings
    {
        public MovieStoreSettings MovieStoreSettings { get; set; }
        public TMDBSettings TMDBSettings { get; set; }
    }
}
