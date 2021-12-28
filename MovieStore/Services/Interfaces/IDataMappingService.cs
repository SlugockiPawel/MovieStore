using System.Threading.Tasks;
using MovieStore.Models.Database;
using MovieStore.Models.TMDB;

namespace MovieStore.Services.Interfaces
{
    public interface IDataMappingService
    {
        Task<Movie> MapMovieDetailAsync(MovieDetail movie);
        ActorDetail MapActorDetail(ActorDetail actor);
    }
}
