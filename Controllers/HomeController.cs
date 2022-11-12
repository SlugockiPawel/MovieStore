using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.Enums;
using MovieStore.Models.ViewModels;
using MovieStore.Services.Interfaces;

namespace MovieStore.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRemoteMovieService _tmdbMovieService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IRemoteMovieService tmdbMovieService)
        {
            _logger = logger;
            _context = context;
            _tmdbMovieService = tmdbMovieService;
        }

        public async Task<IActionResult> Index()
        {
            const int count = 16;
            var data = new LandingPageVM()
            {
                CustomCollections = await _context.Collections
                    .Include(c => c.MovieCollections)
                    .ThenInclude(mc => mc.Movie)
                    .ToListAsync(),
            
                NowPlaying = await _tmdbMovieService.SearchMovieAsync(MovieCategory.now_playing, count),
                Popular = await _tmdbMovieService.SearchMovieAsync(MovieCategory.popular, count),
                TopRated = await _tmdbMovieService.SearchMovieAsync(MovieCategory.top_rated, count),
                Upcoming = await _tmdbMovieService.SearchMovieAsync(MovieCategory.upcoming, count),
            };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
