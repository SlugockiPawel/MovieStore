using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieStore.Data;
using MovieStore.Models.Database;
using MovieStore.Models.Settings;
using MovieStore.Services.Interfaces;

namespace MovieStore.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRemoteMovieService _tmdbMovieService;
        private readonly IDataMappingService _tmdbMappingService;

        public MoviesController(IOptions<AppSettings> appSettings, ApplicationDbContext context, IImageService imageService,
            IRemoteMovieService tmdbMovieService, IDataMappingService tmdbMappingService)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _imageService = imageService;
            _tmdbMovieService = tmdbMovieService;
            _tmdbMappingService = tmdbMappingService;
        }

        public async Task<IActionResult> Import()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int id)
        {
            // if we already have this movie we can just warn the user instead of importing it again
            if (_context.Movies.Any(m => m.MovieId == id))
            {
                var localMovie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
                return RedirectToAction("Details", "Movies", new { id = localMovie.Id, local = true });
            }

            // If there is no such movie in the db, get raw data from the API
            var movieDetail = await _tmdbMovieService.MovieDetailAsync(id);

            // Run the data through the mapping procedure
            var movie = await _tmdbMappingService.MapMovieDetailAsync(movieDetail);

            // Add movie to the database
            await _context.Movies.AddAsync(movie); // _context.Add(movie) ?  TODO CHECK WHAT IS A DIFFERENCE!
            await _context.SaveChangesAsync();

            // Assign the movie to the default 'All' collection
            await AddToMovieCollection(movie.Id, _appSettings.MovieStoreSettings.DefaultCollection.Name);

            return RedirectToAction("Import");
        }

        public async Task<IActionResult> Library()
        {
            var movies = await _context.Movies.ToListAsync();

            return View(movies);
        }

        private async Task AddToMovieCollection(int movieId, string collectionName)
        {
            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Name == collectionName);
            _context.Add(new MovieCollection()
                {
                    CollectionId = collection.Id,
                    MovieId = movieId,
                }
            );
            await _context.SaveChangesAsync();
        }

        private async Task AddToMovieCollection(int movieId, int collectionId)
        {
            _context.Add(new MovieCollection()
                {
                    CollectionId = collectionId,
                    MovieId = movieId,
                }
            );
            await _context.SaveChangesAsync();
        }
    }
}