using Microsoft.AspNetCore.Mvc;
using Msh.WebApp.Models;
using System.Diagnostics;

namespace Msh.WebApp.Controllers
{
    // [Route("bookings")]
    public class BookingsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public BookingsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[Route("")]
        public IActionResult Index()
        {
            return View();
        }

        //[Route("Search")]
		public IActionResult Search()
        {
            return View();
        }

        //[Route("Results")]
		public IActionResult Results()
        {
            return View();
        }

        //[Route("Details")]
		public IActionResult Details()
        {
            return View();
        }

        //[Route("Payment")]
		public IActionResult Payment()
        {
            return View();
        }

        //[Route("Confirmation")]
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
