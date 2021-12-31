using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MovieStore.Enums;
using MovieStore.Models.Settings;
using MovieStore.Models.TMDB;
using MovieStore.Services.Interfaces;

namespace MovieStore.Services
{
    public class TMDBMovieService : IRemoteMovieService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;

        public TMDBMovieService(IOptions<AppSettings> appSettings, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public Task<MovieDetail> MovieDetailAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MovieSearch> SearchMovieAsync(MovieCategory category, int count)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActorDetail> ActorDetailAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}