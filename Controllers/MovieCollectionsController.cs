using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Models.Database;

namespace MovieStore.Controllers
{
    public sealed class MovieCollectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieCollectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? collectionId)
        {
            collectionId ??= (await _context.Collections.FirstOrDefaultAsync(c => c.Name.ToUpper() == "ALL")).Id;

            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name", collectionId);

            var allMoviesIds = await _context.Movies.Select(m => m.Id).ToListAsync();

            var moviesIdsInCollection = await _context.MovieCollections
                .Where(m => m.CollectionId == collectionId)
                .OrderBy(m => m.Order)
                .Select(m => m.MovieId)
                .ToListAsync();

            var moviesIdsNotInCollection = allMoviesIds.Except(moviesIdsInCollection);

            var moviesInCollection = new List<Movie>();
            moviesIdsInCollection.ForEach(movieId => moviesInCollection.Add(_context.Movies.Find(movieId)));

            ViewData["IdsInCollection"] = new MultiSelectList(moviesInCollection, "Id", "Title");

            var moviesNotInCollection = await _context.Movies.AsNoTracking()
                .Where(m => moviesIdsNotInCollection.Contains(m.Id)).ToListAsync();

            ViewData["IdsNotInCollection"] = new MultiSelectList(moviesNotInCollection, "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int collectionId, List<int> idsInCollection)
        {
            var oldRecords = _context.MovieCollections.Where(mc => mc.CollectionId == collectionId);
            _context.MovieCollections.RemoveRange(oldRecords);
            await _context.SaveChangesAsync();

            if (idsInCollection is not null)
            {
                int index = 1;
                idsInCollection.ForEach(movieId =>
                {
                    // TODO debug this code to understand it properly
                    _context.Add(new MovieCollection()
                    {
                        CollectionId = collectionId,
                        MovieId = movieId,
                        Order = index++,
                    });
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { collectionId });
        }
    }
}