using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Config;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API.Ows;

[ApiController]
[Route("api/owsapi")]
public class OwsApiController(IOwsConfigService owsConfigService) : Controller
{

	[HttpGet]
	[Route("OwsConfigMaps")]
	public async Task<IActionResult> OwsConfigMaps()
	{
		var owsConfig = await owsConfigService.GetOwsConfigAsync();
		
		if (owsConfig != null)
		{
			return Ok(new ObjectVm
			{
				Data = owsConfig.SchemeMap
			});
		}

		return Ok(new ObjectVm
		{
			Data = new List<OwsPaymentCodeMap>()
		});
	}

	[HttpPost]
	[Route("OwsConfigMaps")]
	public async Task<IActionResult> OwsConfigMaps([FromBody] OwsConfigVm data)
	{
		try
		{
			await Task.Delay(0);

			var owsConfig = await owsConfigService.GetOwsConfigAsync();
			owsConfig.SchemeMap = data.SchemaMaps;

			await owsConfigService.SaveOwsConfigAsync(owsConfig);

			return Ok(new ObjectVm
			{
				Data = owsConfig.SchemeMap
			});
		}
		catch (Exception ex)
		{
			return Ok(new ObjectVm
			{
				Success = false,
				UserErrorMessage = ex.Message
			});
		}
	}


	[Route("OwsConfigEditTriggers")]
	public async Task<IActionResult> OwsConfigEditTriggers()
	{
		var owsConfig = await owsConfigService.GetOwsConfigAsync();

		if (owsConfig != null)
		{
			return Ok(new ObjectVm
			{
				Data = owsConfig.CriticalErrorTriggers
			});
		}

		return Ok(new ObjectVm
		{
			Data = new List<OwsPaymentCodeMap>()
		});
	}


	[HttpPost]
	[Route("OwsConfigEditTriggers")]
	public async Task<IActionResult> OwsConfigEditTriggers([FromBody] OwsConfigVm data)
	{
		try
		{
			await Task.Delay(0);

			var owsConfig = await owsConfigService.GetOwsConfigAsync();
			owsConfig.CriticalErrorTriggers = data.CriticalErrorTriggers;

			await owsConfigService.SaveOwsConfigAsync(owsConfig);

			var owsConfig2 = await owsConfigService.GetOwsConfigAsync();

			return Ok(new ObjectVm
			{
				Data = owsConfig2.CriticalErrorTriggers
			});
		}
		catch (Exception ex)
		{
			return Ok(new ObjectVm
			{
				Success = false,
				UserErrorMessage = ex.Message
			});
		}
	}
}