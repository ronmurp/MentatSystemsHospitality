using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.Specials;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API.Admin.Hotels;

public partial class HotelApiController
{

	[HttpPost]
	[Route("SpecialDelete")]
	public async Task<IActionResult> SpecialDelete(ApiInput input)
	{
		try
		{
			var items = await specialsRepository.GetData(input.HotelCode);
			var item = items.FirstOrDefault(h => h.Code == input.Code);
			if (item != null)
			{
				items.Remove(item);
				await specialsRepository.Save(items, input.HotelCode);
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
	[Route("SpecialCopy")]
	public async Task<IActionResult> SpecialCopy(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var items = await specialsRepository.GetData(input.HotelCode);
			var item = items.FirstOrDefault(h => h.Code == input.Code);
			if (item != null)
			{
				var newItem = item.Adapt(item);
				newItem.Code = input.NewCode;

				var result = await CheckHotel(input.NewHotelCode);
				if (!result.success)
				{
					return result.fail;
				}

				var newItems = await specialsRepository.GetData(input.NewHotelCode);
				if (newItems.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newItems.Add(newItem);
				await specialsRepository.Save(newItems, input.NewHotelCode);
			}

			return Ok(new ObjectVm());
			
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("SpecialCopyBulk")]
	public async Task<IActionResult> SpecialCopyBulk(ApiInput input)
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
			var newList = new List<Special>();

			var srcItems = await specialsRepository.GetData(input.HotelCode);
			var dstItems = await specialsRepository.GetData(input.NewHotelCode);

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

			await specialsRepository.Save(dstItems, input.NewHotelCode);

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
	[Route("SpecialDeleteBulk")]
	public async Task<IActionResult> SpecialDeleteBulk(ApiInput input)
	{
		try
		{
			var hotels = await hotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await specialsRepository.GetData(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.Code)))
				{
					items.RemoveAt(i);
				}
			}

			await specialsRepository.Save(items, input.HotelCode);

			

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("SpecialsSort")]
	public async Task<IActionResult> SpecialsSort(ApiInput input)
	{
		try
		{
			var hotelCode = input.HotelCode;
			var hotels = await hotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}
			
			var srcItems = await specialsRepository.GetData(hotelCode);

			await specialsRepository.Save(srcItems.OrderBy(e => e.Code).ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}



	[HttpGet]
	[Route("SpecialDates")]
	public async Task<IActionResult> SpecialDates(string code, string hotelCode)
	{
		var items = await specialsRepository.GetData(hotelCode);
		var item = items.FirstOrDefault(h => h.Code == code);
		if (item == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and special {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = item.ItemDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}

	[HttpPost]
	[Route("SpecialDates")]
	public async Task<IActionResult> SpecialDates([FromBody] ItemDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var items = await specialsRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].ItemDates = data.Dates;
				await specialsRepository.Save(items, data.HotelCode);
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
	[Route("SpecialOptions")]
	public async Task<IActionResult> SpecialOptions(string code, string hotelCode)
	{
		var items = await specialsRepository.GetData(hotelCode);
		var item = items.FirstOrDefault(h => h.Code == code);
		if (item == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and special {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Options = item.Options
			}
		});
	}

	[HttpPost]
	[Route("SpecialOptions")]
	public async Task<IActionResult> SpecialOptions([FromBody] SpecialOptionsVm data)
	{
		try
		{
			await Task.Delay(0);

			var items = await specialsRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].Options = data.Options;
				await specialsRepository.Save(items, data.HotelCode);
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
	[Route("SpecialRoomTypesSave")]
	public async Task<IActionResult> SpecialRoomTypesSave([FromBody] ApiInput data)
	{
		try
		{
			await Task.Delay(0);

			var items = await specialsRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].RoomTypeCodes = data.CodeList;
				await specialsRepository.Save(items, data.HotelCode);
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
	[Route("SpecialRatePlansSave")]
	public async Task<IActionResult> SpecialRatePlansSave([FromBody] ApiInput data)
	{
		try
		{
			await Task.Delay(0);

			var items = await specialsRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].RatePlanCodes = data.CodeList;
				await specialsRepository.Save(items, data.HotelCode);
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
	[Route("SpecialRatePlans")]
	public async Task<IActionResult> SpecialRatePlans(string code, string hotelCode)
	{
		var items = await specialsRepository.GetData(hotelCode);
		var item = items.FirstOrDefault(h => h.Code == code);
		if (item == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and special {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Options = item.RatePlanCodes
			}
		});
	}


	[HttpPost]
	[Route("SpecialRatePlans")]
	public async Task<IActionResult> SpecialRatePlans([FromBody] ApiInput data)
	{
		try
		{
			await Task.Delay(0);

			var items = await specialsRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].RatePlanCodes = data.CodeList;
				await specialsRepository.Save(items, data.HotelCode);
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