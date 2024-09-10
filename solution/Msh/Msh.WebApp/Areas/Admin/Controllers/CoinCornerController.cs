using Microsoft.AspNetCore.Mvc;
using Msh.Pay.CoinCorner.Models;
using Msh.Pay.CoinCorner.Services;

namespace Msh.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/coincorner")]
public class CoinCornerController(
	ILogger<WebApp.Controllers.HomeController> logger,
	ICoinCornerCacheService coinCornerCacheService,
	ICoinCornerRepoService coinCornerRepoService)
	: Controller
{
	[Route("")]
    public async Task<IActionResult> Index()
    {
	    await Task.Delay(0);

        return View();
    }

    [Route("Config")]
    public async Task<IActionResult> Config()
    {
		try
		{
			await Task.Delay(0);

			var config = coinCornerRepoService.GetConfig();

			return View(config);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			return View(new CoinCornerConfig());
		}
	}

    [HttpPost]
    [Route("Config")]
    public async Task<IActionResult> Config([FromForm] CoinCornerConfig config, string command)
    {
	    try
	    {
		    await Task.Delay(0);

		    if (command == "Save")
		    {
			    coinCornerRepoService.SaveConfig(config);
			}
			else if (command == "Publish")
			{
				coinCornerCacheService.ReloadConfig();
			}

		    return RedirectToAction("Config");
	    }
	    catch (Exception ex)
	    {
			logger.LogError($"{ex.Message}");
		}

		return View(config);

	}
    
    
    [Route("Global")]
    public async Task<IActionResult> Global()
    {
	    try
	    {
		    await Task.Delay(0);

		    var global = coinCornerRepoService.GetGlobal();

		    return View(global);
	    }
	    catch (Exception ex)
	    {
		    logger.LogError($"{ex.Message}");
		    return View(new CoinCornerGlobal());
	    }
    }

    [HttpPost]
    [Route("Global")]
    public async Task<IActionResult> Global([FromForm] CoinCornerGlobal global, string command)
    {
	    try
	    {
		    await Task.Delay(0);

		    if (command == "Save")
		    {
			    coinCornerRepoService.SaveGlobal(global);
		    }
		    else if (command == "Publish")
		    {
			    coinCornerCacheService.ReloadGlobal();
		    }

		    return RedirectToAction("Global");
	    }
	    catch (Exception ex)
	    {
		    logger.LogError($"{ex.Message}");
	    }

	    return View(global);

    }


}