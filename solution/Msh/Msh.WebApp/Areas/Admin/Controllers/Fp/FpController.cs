using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.HotelCache.Models;
using Msh.Opera.Ows.Cache;
using Msh.Opera.Ows.Models;
using Msh.Pay.FreedomPay.Models.Configuration;
using Msh.Pay.FreedomPay.Services;

namespace Msh.WebApp.Areas.Admin.Controllers.Fp;

[Authorize]
[Area("Admin")]
[Route("admin/fp")]
public class FpController(IFpRepoService fpRepoService) : Controller
{

	[HttpGet]
	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);
		return View();
	}

	[HttpGet]
	[Route("FpConfigEdit")]
	public async Task<IActionResult> FpConfigEdit(bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;



		var fpConfig = await fpRepoService.GetFpConfig();
		if (fpConfig == null)
		{
			fpConfig = new FpConfig();
			await fpRepoService.SaveFpConfig(fpConfig);
		}

		return View(fpConfig);

	}

	[HttpPost]
	[Route("FpConfigEdit")]
	public async Task<IActionResult> FpConfigEdit([FromForm] FpConfig owsConfig)
	{

		if (ModelState.IsValid)
		{

			var fpConfigCurrent = await fpRepoService.GetFpConfig();


			await fpRepoService.SaveFpConfig(owsConfig);

			return RedirectToAction(nameof(FpConfigEdit), new { IsSuccess = true });

		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View();
		}
	}

}