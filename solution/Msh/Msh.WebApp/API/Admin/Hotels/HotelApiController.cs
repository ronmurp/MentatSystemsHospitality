using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

[ApiController]
[Route("api/hotelapi")]
public partial class HotelApiController : PrivateApiController
{
	private readonly IUserService _userService;
	private readonly ITestModelRepository _testModelRepository;
	private const string ModelName = "Hotel";

	//public HotelApiController(IConfigRepository configRepository,
	//	IHotelRepository hotelRepository,
	//	ITestModelRepository testModelRepository,
	//	IUserService userService) : PrivateApiController(hotelRepository)
	//{

	//}
	public HotelApiController(IConfigRepository configRepository,
		IHotelRepository hotelRepository,
		IUserService userService,
		ITestModelRepository testModelRepository) : base(hotelRepository)
	{
		_userService = userService;
		_testModelRepository = testModelRepository;
	}

	[HttpGet]
	[Route("HotelStayDates")]
	public async Task<IActionResult> HotelStayDates(string hotelCode)
	{
		var hotels = await HotelRepository.GetData();
		
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

			var hotels = await HotelRepository.GetData();

			var index = hotels.FindIndex(h => h.HotelCode == data.HotelCode);

			if (index >= 0)
			{
				hotels[index].StayDates = data.Dates;
				await HotelRepository.Save(hotels);
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
		var hotels = await HotelRepository.GetData();

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

			var hotels = await HotelRepository.GetData();
			var index = hotels.FindIndex(h => h.HotelCode == data.HotelCode);

			if (index >= 0)
			{
				hotels[index].BookDates = data.Dates;
				await HotelRepository.Save(hotels);
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
			var hotels = await HotelRepository.GetData();

			var hotel = hotels.FirstOrDefault(h => h.HotelCode == input.HotelCode);
			if (hotel != null)
			{
				hotels.Remove(hotel);
				await HotelRepository.Save(hotels);
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
			var srcItems = await HotelRepository.GetData();
			var srcItem = srcItems.FirstOrDefault(h => h.HotelCode == input.Code);
			if (srcItem != null)
			{
				var newItem = srcItem.Adapt(srcItem);
				newItem.HotelCode = input.NewCode;

				var newItems = await HotelRepository.GetData();
				if (newItems.Any(c => c.HotelCode.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newItems.Add(newItem);
				await HotelRepository.Save(newItems);
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
			var items = await HotelRepository.GetData();

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.HotelCode)))
				{
					items.RemoveAt(i);
				}
			}

			await HotelRepository.Save(items);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}



	[HttpPost]
	[Route("TestModelDelete")]
	public async Task<IActionResult> TestModelDelete([FromBody] object obj, [FromQuery]string code)
	{
		try
		{
			await Task.Delay(0);

			var testModels = await _testModelRepository.GetData();
			if (testModels.Any(m => m.Code == code))
			{
				var tm = testModels.First(m => m.Code == code);
				testModels.Remove(tm);
				await _testModelRepository.Save(testModels);
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


}