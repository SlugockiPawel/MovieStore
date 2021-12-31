using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
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

        public async Task<MovieDetail> MovieDetailAsync(int id)
        {
            // Setup a default instance of MovieDetail
            MovieDetail movieDetail = new();

            // Assemble full request uri string
            var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{id}";

            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _appSettings.MovieStoreSettings.TmDbApiKey },
                { "language", _appSettings.TMDBSettings.QueryOptions.Language },
                { "append_to_response", _appSettings.TMDBSettings.QueryOptions.AppendToResponse }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            // Create a client and execute the request
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            // Deserialize into MovieDetail
            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieDetail));
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                movieDetail = dcjs.ReadObject(responseStream) as MovieDetail;
            }

            return movieDetail;
        }

        public async Task<MovieSearch> SearchMovieAsync(MovieCategory category, int count)
        {
            // Setup a default instance of MovieSearch
            MovieSearch movieSearch = new();

            // Assemble full request uri string
            var query = $"{_appSettings.TMDBSettings.BaseUrl}/movie/{category}";

            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _appSettings.MovieStoreSettings.TmDbApiKey },
                { "language", _appSettings.TMDBSettings.QueryOptions.Language },
                { "page", _appSettings.TMDBSettings.QueryOptions.Page }
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            // Create a client and execute the request
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            // Return MovieSearch object
            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(MovieSearch));
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                movieSearch = (MovieSearch)dcjs.ReadObject(responseStream);
                movieSearch.results = movieSearch.results.Take(count).ToArray();
                movieSearch.results.ToList().ForEach(r =>
                    r.poster_path =
                        $"{_appSettings.TMDBSettings.BaseImagePath}/{_appSettings.MovieStoreSettings.DefaultPosterSize}/{r.poster_path}");
            }

            return movieSearch;
        }

        public async Task<ActorDetail> ActorDetailAsync(int id)
        {
            // Setup a default instance of ActorDetail
            ActorDetail actorDetail = new();

            // Assemble full request uri string
            var query = $"{_appSettings.TMDBSettings.BaseUrl}/person/{id}";

            var queryParams = new Dictionary<string, string>()
            {
                { "api_key", _appSettings.MovieStoreSettings.TmDbApiKey },
                { "language", _appSettings.TMDBSettings.QueryOptions.Language },
            };

            var requestUri = QueryHelpers.AddQueryString(query, queryParams);

            // Create a client and execute the request
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request);

            // Deserialize into MovieDetail
            if (response.IsSuccessStatusCode)
            {
                var dcjs = new DataContractJsonSerializer(typeof(ActorDetail));
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                actorDetail = dcjs.ReadObject(responseStream) as ActorDetail;
            }

            return actorDetail;
        }
    }
}