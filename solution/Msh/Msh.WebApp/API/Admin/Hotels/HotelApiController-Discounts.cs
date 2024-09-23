using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Models.Hotels;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API.Admin.Hotels;

public partial class HotelApiController
{

	

	[HttpPost]
	[Route("DiscountCopy")]
	public async Task<IActionResult> DiscountCopy(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var discounts = await hotelsRepoService.GetDiscountCodesAsync(input.HotelCode);
			var discount = discounts.FirstOrDefault(h => h.Code == input.Code);
			if (discount != null)
			{
				var newDiscount = discount.Adapt(discount);
				newDiscount.Code = input.NewCode;

				var result = await CheckHotel(input.NewHotelCode);
				if (!result.success)
				{
					return result.fail;
				}

				var newDiscounts = await hotelsRepoService.GetDiscountCodesAsync(input.NewHotelCode);
				if (newDiscounts.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newDiscounts.Add(newDiscount);
				await hotelsRepoService.SaveDiscountCodesAsync(newDiscounts, input.NewHotelCode);
			}

			return Ok(new ObjectVm());
			
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("DiscountCopyBulk")]
	public async Task<IActionResult> DiscountCopyBulk(ApiInput input)
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
			var newList = new List<DiscountCode>();

			var srcItems = await hotelsRepoService.GetDiscountCodesAsync(input.HotelCode);
			var dstItems = await hotelsRepoService.GetDiscountCodesAsync(input.NewHotelCode);

			foreach (var code in input.CodeList)
			{
				var item = srcItems.FirstOrDefault(h => h.Code == code);
				if (item != null)
				{
					if (dstItems.Any(e => e.Code == item.Code))
					{
						// Already exists
						missingList.Add(item.Code);
						continue;
					}
					newList.Add(item);
				}
			}

			dstItems.AddRange(newList);

			await hotelsRepoService.SaveDiscountCodesAsync(dstItems, input.NewHotelCode);

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
	[Route("DiscountDeleteBulk")]
	public async Task<IActionResult> DiscountDeleteBulk(ApiInput input)
	{
		try
		{
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await hotelsRepoService.GetDiscountCodesAsync(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.Code)))
				{
					items.RemoveAt(i);
				}
			}

			await hotelsRepoService.SaveDiscountCodesAsync(items, input.HotelCode);

			

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("DiscountsSort")]
	public async Task<IActionResult> DiscountsSort(ApiInput input)
	{
		try
		{
			var hotelCode = input.HotelCode;
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}
			
			var srcItems = await hotelsRepoService.GetDiscountCodesAsync(hotelCode);

			await hotelsRepoService.SaveDiscountCodesAsync(srcItems.OrderBy(e => e.Code).ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}



	[HttpGet]
	[Route("DiscountOfferDates")]
	public async Task<IActionResult> DiscountOfferDates(string code, string hotelCode)
	{
		var items = await hotelsRepoService.GetDiscountCodesAsync(hotelCode);
		var item = items.FirstOrDefault(h => h.Code == code);
		if (item == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = item.OfferDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}

	[HttpPost]
	[Route("DiscountOfferDates")]
	public async Task<IActionResult> DiscountOfferDates([FromBody] ItemDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var items = await hotelsRepoService.GetDiscountCodesAsync(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].OfferDates = data.Dates;
				await hotelsRepoService.SaveDiscountCodesAsync(items, data.HotelCode);
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


	[HttpGet]
	[Route("DiscountBookDates")]
	public async Task<IActionResult> DiscountBookDates(string code, string hotelCode)
	{
		var items = await hotelsRepoService.GetDiscountCodesAsync(hotelCode);
		var item = items.FirstOrDefault(h => h.Code == code);
		if (item == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = item.BookDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}

	[HttpPost]
	[Route("DiscountBookDates")]
	public async Task<IActionResult> DiscountBookDates([FromBody] ItemDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var items = await hotelsRepoService.GetDiscountCodesAsync(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].BookDates = data.Dates;
				await hotelsRepoService.SaveDiscountCodesAsync(items, data.HotelCode);
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


	[HttpPost]
	[Route("DiscountRatePlansSaveEnable")]
	public async Task<IActionResult> DiscountRatePlansSaveEnable([FromBody] ApiInput data)
	{
		try
		{
			await Task.Delay(0);

			var items = await hotelsRepoService.GetDiscountCodesAsync(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].EnabledHotelPlans = data.CodeList;
				await hotelsRepoService.SaveDiscountCodesAsync(items, data.HotelCode);
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


	[HttpPost]
	[Route("DiscountRatePlansSaveDisable")]
	public async Task<IActionResult> DiscountRatePlansSaveDisable([FromBody] ApiInput data)
	{
		try
		{
			await Task.Delay(0);

			var items = await hotelsRepoService.GetDiscountCodesAsync(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].DisabledHotelPlans = data.CodeList;
				await hotelsRepoService.SaveDiscountCodesAsync(items, data.HotelCode);
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