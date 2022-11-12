using System.Collections.Generic;
using MovieStore.Models.Database;
using MovieStore.Models.TMDB;

namespace MovieStore.Models.ViewModels
{
    public sealed class LandingPageVM
    {
        public List<Collection> CustomCollections { get; set; }
        public MovieSearch NowPlaying { get; set; }
        public MovieSearch Popular { get; set; }
        public MovieSearch TopRated { get; set; }
        public MovieSearch Upcoming { get; set; }
    }
}
