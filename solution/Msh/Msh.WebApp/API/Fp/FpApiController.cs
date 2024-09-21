using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.Opera.Ows.Models;
using Msh.Pay.FreedomPay.Models.Configuration;
using Msh.Pay.FreedomPay.Services;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Fp;

[ApiController]
[Route("api/fpapi")]
public class FpApiController(IFpRepoService fpRepoService) : Controller
{


	[HttpGet]
	[Route("FpErrorBankList")]
	public async Task<IActionResult> FpErrorBankList()
	{
		var list = await fpRepoService.GetFpErrorCodeBank();
		
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
	[Route("FpErrorBankList")]
	public async Task<IActionResult> FpErrorBankList([FromBody] FpApiVm data)
	{
		try
		{
			await Task.Delay(0);

			
			await fpRepoService.SaveFpErrorCodeBank(data.ErrorBankList);

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