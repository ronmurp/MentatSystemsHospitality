using Microsoft.AspNetCore.Mvc;

namespace Msh.WebApp.Controllers.Admin.CoinCorner;

[Route("admin/payments/coincorner")]
public class CoinCornerController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public CoinCornerController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("")]
    public IActionResult Index()
    {
        return View("~/Views/Admin/CoinCorner/Index.cshtml");
    }

    [Route("Config")]
    public IActionResult Xxx()
    {
        return View("~/Views/Admin/CoinCorner/Config.cshtml");
    }
}