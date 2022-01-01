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

        }
    }
}
