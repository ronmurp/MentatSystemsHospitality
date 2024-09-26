using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.RatePlans;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API.Admin.Hotels;


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

			var ratePlans = await ratePlanRepository.GetData(input.HotelCode);
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

				var newRatePlans = await ratePlanRepository.GetData(input.NewHotelCode);
				if (newRatePlans.Any(c => c.RatePlanCode.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newRatePlans.Add(newRatePlan);
				await ratePlanRepository.Save(newRatePlans, input.NewHotelCode);
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
			var ratePlans = await ratePlanRepository.GetData(input.HotelCode);
			var ratePlan = ratePlans.FirstOrDefault(h => h.RatePlanCode == input.Code);
			if (ratePlan != null)
			{
				ratePlans.Remove(ratePlan);
				await ratePlanRepository.Save(ratePlans, input.HotelCode);
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

			var hotels = await hotelRepository.GetData();

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

			var srcItems = await ratePlanRepository.GetData(input.HotelCode);
			var dstItems = await ratePlanRepository.GetData(input.NewHotelCode);

			foreach (var code in input.CodeList)
			{
				var extra = srcItems.FirstOrDefault(h => h.RatePlanCode == code);
				if (extra != null)
				{
					if (dstItems.Any(e => e.RatePlanCode == extra.RatePlanCode))
					{
						// Already exists
						missingList.Add(extra.RatePlanCode);
						continue;
					}
					newList.Add(extra);
				}
			}

			dstItems.AddRange(newList);

			await ratePlanRepository.Save(dstItems, input.NewHotelCode);

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
	[Route("RatePlanDeleteBulk")]
	public async Task<IActionResult> RatePlanDeleteBulk(ApiInput input)
	{
		try
		{
			var hotels = await hotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await ratePlanRepository.GetData(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.RatePlanCode)))
				{
					items.RemoveAt(i);
				}
			}

			await ratePlanRepository.Save(items, input.HotelCode);



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
			var hotels = await hotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcExtras = await ratePlanRepository.GetData(hotelCode);

			await ratePlanRepository.Save(srcExtras.OrderBy(e => e.RatePlanCode)
				.ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

}