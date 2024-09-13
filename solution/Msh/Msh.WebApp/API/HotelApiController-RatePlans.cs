using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API;


public partial class HotelApiController
{


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
	
}