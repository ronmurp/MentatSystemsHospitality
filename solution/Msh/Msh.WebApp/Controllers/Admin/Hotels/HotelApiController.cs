using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Services;

namespace Msh.WebApp.Controllers.Admin.Hotels;

[ApiController]
[Route("api/hotelapi")]
public class HotelApiController(IHotelsRepoService hotelsRepoService) : Controller
{

	[HttpGet]
	[Route("hotelSave")]
	public async Task<IActionResult> HotelSave()
	{
		try
		{
			await Task.Delay(0);

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
	[Route("hotelSave")]
	public async Task<IActionResult> HotelSave(Hotel hotel)
	{
		try
		{
			await Task.Delay(0);

			return Ok(new ObjectVm
			{
				Data = hotel
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

}