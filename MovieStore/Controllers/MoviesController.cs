using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
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