using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using MovieStore.Enums;
using MovieStore.Models.Database;
using MovieStore.Models.Settings;
using MovieStore.Models.TMDB;
using MovieStore.Services.Interfaces;

namespace MovieStore.Services
{
    public class TMDBMappingService : IDataMappingService
    {
        public Task<Movie> MapMovieDetailAsync(MovieDetail movie)
        {
            throw new System.NotImplementedException();
        }

        public ActorDetail MapActorDetail(ActorDetail actor)
        {
            throw new System.NotImplementedException();

        private string BuildTrailerPath(Videos videos)
        {
            var videoKey = videos.results.FirstOrDefault(r => r.type.ToLower().Trim() == "trailer" && r.key != "")?.key;
            return string.IsNullOrWhiteSpace(videoKey)
                ? videoKey
                : $"{_appSettings.TMDBSettings.BaseYouTubePath}{videoKey}";
        }

        private MovieRating GetRating(Release_Dates dates)
        {
            var movieRating = MovieRating.NR;
            var certification = dates.results.FirstOrDefault(r => r.iso_3166_1 == "US");
            if (certification is not null)
            {
                var apiRating = certification.release_dates.FirstOrDefault(c => c.certification != "")?.certification
                    .Replace("-", "");

                if (!string.IsNullOrWhiteSpace(apiRating))
                {
                    movieRating = (MovieRating)Enum.Parse(typeof(MovieRating), apiRating, true);
                }
            }

            return movieRating;
        }

        private string BuildCastImage(string profilePath)
        {
            if (string.IsNullOrWhiteSpace(profilePath))
                return _appSettings.MovieStoreSettings.DefaultCastImage;

            return
                $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieStoreSettings.DefaultPosterSize}/{profilePath}";
        }
    }
}