using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.RatePlans;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API;


public partial class HotelApiController
{
	
	[HttpPost]
	[Route("RatePlanCopy")]
	public async Task<IActionResult> RatePlanCopy(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var ratePlans = await hotelsRepoService.GetRatePlansAsync(input.HotelCode);
			var ratePlan = ratePlans.FirstOrDefault(h => h.RatePlanCode == input.Code);
			if (ratePlan != null)
			{
				var newRatePlan = ratePlan.Adapt(ratePlan);
				newRatePlan.RatePlanCode = input.NewCode;

				var result = await CheckHotel(input.NewHotelCode);
				if (!result.success)
				{
					return GetFail("The hotel does not exist.");
				}

				var newRatePlans = await hotelsRepoService.GetRatePlansAsync(input.NewHotelCode);
				if (newRatePlans.Any(c => c.RatePlanCode.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newRatePlans.Add(newRatePlan);
				await hotelsRepoService.SaveRatePlansAsync(newRatePlans, input.NewHotelCode);
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("RatePlanDelete")]
	public async Task<IActionResult> RatePlanDelete(ApiInput input)
	{
		try
		{
			var ratePlans = await hotelsRepoService.GetRatePlansAsync(input.HotelCode);
			var ratePlan = ratePlans.FirstOrDefault(h => h.RatePlanCode == input.Code);
			if (ratePlan != null)
			{
				ratePlans.Remove(ratePlan);
				await hotelsRepoService.SaveRatePlansAsync(ratePlans, input.HotelCode);
			}

			return Ok(new ObjectVm());
			
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("RatePlanCopyBulk")]
	public async Task<IActionResult> RatePlanCopyBulk(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.NewHotelCode)))
			{
				return GetFail($"Invalid destination hotel code {input.NewHotelCode}");
			}

			var missingList = new List<string>();
			var newList = new List<RoomRatePlan>();

			var srcExtras = await hotelsRepoService.GetRatePlansAsync(input.HotelCode);
			var dstExtras = await hotelsRepoService.GetRatePlansAsync(input.NewHotelCode);

			foreach (var code in input.CodeList)
			{
				var extra = srcExtras.FirstOrDefault(h => h.RatePlanCode == code);
				if (extra != null)
				{
					if (dstExtras.Any(e => e.RatePlanCode == extra.RatePlanCode))
					{
						// Already exists
						missingList.Add(extra.RatePlanCode);
						continue;
					}
					newList.Add(extra);
				}
			}

			dstExtras.AddRange(newList);

			await hotelsRepoService.SaveRatePlansAsync(dstExtras, input.NewHotelCode);

			if (missingList.Count > 0)
			{
				var list = string.Join(",", missingList);
				return GetFail($"The following codes already exist in the destination hotel: {list}");

			}

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("RatePlansSort")]
	public async Task<IActionResult> RatePlansSort(ApiInput input)
	{
		try
		{
			var hotelCode = input.HotelCode;
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcExtras = await hotelsRepoService.GetRatePlansAsync(hotelCode);

			await hotelsRepoService.SaveRatePlansAsync(srcExtras.OrderBy(e => e.RatePlanCode)
				.ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

}