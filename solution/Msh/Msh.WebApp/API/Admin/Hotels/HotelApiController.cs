using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API.Admin.Hotels;

public partial class HotelApiController
{
	private async Task<IActionResult> GetConfig(Type classType)
	{
		try
		{
			await Task.Delay(0);

			var propService = new PropertyValueService();
			var list = propService.GetProperties(classType);
			return Ok(new ObjectVm
			{
				Data = list
			});
		}
		catch (Exception ex)
		{
			return Ok(new ObjectVm
			{

				UserErrorMessage = ex.Message
			});
		}
	}

	/// <summary>
	/// During a copy, are the new codes the same as the old codes
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	private bool SameCodes(ApiInput input) =>
		input.NewCode.Equals(input.Code, StringComparison.InvariantCultureIgnoreCase)
		&& input.NewHotelCode.Equals(input.HotelCode, StringComparison.InvariantCultureIgnoreCase);

	private IActionResult GetFail(string message)
	{
		return Ok(new ObjectVm { Success = false, UserErrorMessage = message });
	}

	private async Task<(IActionResult fail, bool success)> CheckHotel(string hotelCode)
	{
		var hotels = await hotelRepository.GetData();
		if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
		{
			return (GetFail($"The hotel does not exist: {hotelCode}"), false);
		}

		return (Ok(new ObjectVm()), true);
	}

	
}