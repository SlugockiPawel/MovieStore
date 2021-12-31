using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MovieStore.Models.Database;
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
        }
    }
}
