namespace MovieStore.Models.Settings
{
    public sealed class TMDBSettings
    {
        public string BaseUrl { get; set; }
        public string BaseImagePath { get; set; }
        public string BaseYouTubePath { get; set; }
        public QueryOptions QueryOptions { get; set; }
    }

    public sealed class QueryOptions   
    {
        public string Language { get; set; }
        public string AppendToResponse { get; set; }
        public string Page { get; set; }
    }
}