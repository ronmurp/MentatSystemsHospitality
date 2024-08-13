using Microsoft.AspNetCore.Mvc;
using Msh.WebApp.Models;
using System.Diagnostics;

namespace Msh.WebApp.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public BookingsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Confirmation()
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
