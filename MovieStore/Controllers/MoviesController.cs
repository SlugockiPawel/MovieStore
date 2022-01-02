using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
