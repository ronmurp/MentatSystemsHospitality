using Microsoft.AspNetCore.Mvc;
using Msh.Pay.CoinCorner.Models;
using Msh.Pay.CoinCorner.Services;

namespace Msh.WebApp.Controllers.Admin.CoinCorner;

[Route("admin/payments/coincorner")]
public class CoinCornerController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICoinCornerCacheService _coinCornerCacheService;
    private readonly ICoinCornerRepoService _coinCornerRepoService;

    public CoinCornerController(ILogger<HomeController> logger, 
	    ICoinCornerCacheService coinCornerCacheService,
	    ICoinCornerRepoService coinCornerRepoService)
    {
	    _logger = logger;
	    _coinCornerCacheService = coinCornerCacheService;
	    _coinCornerRepoService = coinCornerRepoService;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
	    await Task.Delay(0);

        return View("~/Views/Admin/CoinCorner/Index.cshtml");
    }

    [Route("Config")]
    public async Task<IActionResult> Config()
    {
	    await Task.Delay(0);

	    var config = _coinCornerRepoService.GetConfig();

		return View("~/Views/Admin/CoinCorner/Config.cshtml", config);
    }

    [HttpPost]
    [Route("ConfigEdit")]
    public async Task<IActionResult> ConfigEdit([FromForm] CoinCornerConfig config)
    {
	    await Task.Delay(0);

	    _coinCornerRepoService.SaveConfig(config);


		return RedirectToAction("Config");
	    // return View("~/Views/Admin/CoinCorner/Config.cshtml", config);
    }
}