using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Hotels;

[ApiController]
[Route("api/hotelapi")]
public partial class HotelApiController(IHotelsRepoService hotelsRepoService, IUserService userService) : Controller
{
	[HttpPost]
	[Route("HotelsPublish")]
	public async Task<IActionResult> HotelsPublish()
	{
		var userId = userService.GetUserId();
		if (string.IsNullOrEmpty(userId))
		{
			return GetFail("You must be signed-in to perform this action.");
		}
		var result = await hotelsRepoService.PublishHotelsAsync(userId);
		if (!result)
		{
			return GetFail("The publish operation failed. The record may be locked.");
		}

		return Ok(new ObjectVm());
	}


	[HttpGet]
	[Route("HotelStayDates")]
	public async Task<IActionResult> HotelStayDates(string hotelCode)
	{
		var hotels = await hotelsRepoService.GetHotelsAsync();
		var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
		if (hotel == null)
		{
			return Ok(new ObjectVm
			{
				Success = false,
				UserErrorMessage = $"Dates not found for hotel code {hotelCode}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = hotel.StayDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}

	
	[HttpPost]
	[Route("HotelStayDates")]
	public async Task<IActionResult> HotelStayDates([FromBody] HotelDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var hotels = await hotelsRepoService.GetHotelsAsync();
			var index = hotels.FindIndex(h => h.HotelCode == data.HotelCode);

			if (index >= 0)
			{
				hotels[index].StayDates = data.Dates;
				await hotelsRepoService.SaveHotelsAsync(hotels);
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
	[Route("HotelBookDates")]
	public async Task<IActionResult> HotelBookDates(string hotelCode)
	{
		var hotels = await hotelsRepoService.GetHotelsAsync();
		var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
		if (hotel == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel code {hotelCode}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = hotel.BookDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}


	[HttpPost]
	[Route("HotelBookDates")]
	public async Task<IActionResult> HotelBookDates([FromBody] HotelDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var hotels = await hotelsRepoService.GetHotelsAsync();
			var index = hotels.FindIndex(h => h.HotelCode == data.HotelCode);

			if (index >= 0)
			{
				hotels[index].BookDates = data.Dates;
				await hotelsRepoService.SaveHotelsAsync(hotels);
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
	[Route("HotelDelete")]
	public async Task<IActionResult> HotelDelete(ApiInput input)
	{
		try
		{
			var hotels = await hotelsRepoService.GetHotelsAsync();
			var hotel = hotels.FirstOrDefault(h => h.HotelCode == input.HotelCode);
			if (hotel != null)
			{
				hotels.Remove(hotel);
				await hotelsRepoService.SaveHotelsAsync(hotels);
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
	[Route("HotelCopy")]
	public async Task<IActionResult> HotelCopy(ApiInput input)
	{
		try
		{
			var srcItems = await hotelsRepoService.GetHotelsAsync();
			var srcItem = srcItems.FirstOrDefault(h => h.HotelCode == input.Code);
			if (srcItem != null)
			{
				var newItem = srcItem.Adapt(srcItem);
				newItem.HotelCode = input.NewCode;

				var newItems = await hotelsRepoService.GetHotelsAsync();
				if (newItems.Any(c => c.HotelCode.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newItems.Add(newItem);
				await hotelsRepoService.SaveHotelsAsync(newItems);
			}

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("HotelDeleteBulk")]
	public async Task<IActionResult> HotelDeleteBulk(ApiInput input)
	{
		try
		{
			var items = await hotelsRepoService.GetHotelsAsync();

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.HotelCode)))
				{
					items.RemoveAt(i);
				}
			}

			await hotelsRepoService.SaveHotelsAsync(items);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}



	[HttpPost]
	[Route("testModelDelete")]
	public async Task<IActionResult> TestModelDelete([FromBody] object obj, [FromQuery]string code)
	{
		try
		{
			await Task.Delay(0);

			var testModels = await hotelsRepoService.GetTestModelsAsync();
			if (testModels.Any(m => m.Code == code))
			{
				var tm = testModels.First(m => m.Code == code);
				testModels.Remove(tm);
				await hotelsRepoService.SaveTestModelsAsync(testModels);
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


	[HttpGet]
	[Route("hotelConfig")]
	public async Task<IActionResult> HotelConfig()
	{
		return await GetConfig(typeof(Hotel));
	}

	[HttpGet]
	[Route("roomTypeConfig")]
	public async Task<IActionResult> RoomTypeConfig()
	{
		return await GetConfig(typeof(RoomType));
	}

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
		var hotels = await hotelsRepoService.GetHotelsAsync();
		if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
		{
			return (GetFail($"The hotel does not exist: {hotelCode}"), false);
		}

		return (Ok(new ObjectVm()), true);
	}
}