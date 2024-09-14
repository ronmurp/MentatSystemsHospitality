using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
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
	
}