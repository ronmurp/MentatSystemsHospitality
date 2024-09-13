using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.Hotels;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API;

public partial class HotelApiController
{

	[HttpPost]
	[Route("ExtraDelete")]
	public async Task<IActionResult> ExtraDelete(ApiInput input)
	{
		try
		{
			var extras = await hotelsRepoService.GetExtrasAsync(input.HotelCode);
			var extra = extras.FirstOrDefault(h => h.Code == input.Code);
			if (extra != null)
			{
				extras.Remove(extra);
				await hotelsRepoService.SaveExtrasAsync(extras, input.HotelCode);
			}

			return Ok(new ObjectVm
			{

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


	[HttpPost]
	[Route("ExtraCopy")]
	public async Task<IActionResult> ExtraCopy(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var extras = await hotelsRepoService.GetExtrasAsync(input.HotelCode);
			var extra = extras.FirstOrDefault(h => h.Code == input.Code);
			if (extra != null)
			{
				var newExtra = extra.Adapt(extra);
				newExtra.Code = input.NewCode;

				var result = await CheckHotel(input);
				if (!result.success)
				{
					return GetFail("The hotel does not exist.");
				}

				var newExtras = await hotelsRepoService.GetExtrasAsync(input.NewHotelCode);
				if (newExtras.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newExtras.Add(newExtra);
				await hotelsRepoService.SaveExtrasAsync(newExtras, input.NewHotelCode);
			}

			return Ok(new ObjectVm());
			
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}


	[HttpGet]
	[Route("ExtraDates")]
	public async Task<IActionResult> ExtraDates(string code, string hotelCode)
	{
		var extras = await hotelsRepoService.GetExtrasAsync(hotelCode);
		var extra = extras.FirstOrDefault(h => h.Code == code);
		if (extra == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and extra {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = extra.ItemDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}

	[HttpPost]
	[Route("ExtraDates")]
	public async Task<IActionResult> ExtraDates([FromBody] ItemDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var extras = await hotelsRepoService.GetExtrasAsync(data.HotelCode);
			var index = extras.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				extras[index].ItemDates = data.Dates;
				await hotelsRepoService.SaveExtrasAsync(extras, data.HotelCode);
			}

			return Ok(new ObjectVm
			{
				Data = new Hotel()
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