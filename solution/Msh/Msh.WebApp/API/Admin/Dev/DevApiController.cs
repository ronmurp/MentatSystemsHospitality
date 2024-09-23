using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.Opera.Ows.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Dev;

[ApiController]
[Route("api/devapi")]
public class DevApiController(IConfigStateRepo configStateRepo) : Controller
{


	[HttpGet]
	[Route("ConfigStateList")]
	public async Task<IActionResult> ConfigStateList()
	{
		var list = await configStateRepo.GetConfigState();
		
		if (list != null)
		{
			return Ok(new ObjectVm
			{
				Data = list
			});
		}

		return Ok(new ObjectVm
		{
			Data = new List<OwsPaymentCodeMap>()
		});
	}

	[HttpPost]
	[Route("ConfigStateList")]
	public async Task<IActionResult> ConfigStateList([FromBody] ConfigStateVm data)
	{
		try
		{
			await Task.Delay(0);

			
			await configStateRepo.SaveConfigState(data.ConfigStates);

			return Ok(new ObjectVm
			{
				Data = data
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