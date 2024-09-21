using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models.Ows;
using System.Security.Authentication;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Services.CustomTest;
using Msh.Opera.Ows.Cache;

namespace Msh.WebApp.API.Ows;

[ApiController]
[Route("api/owsapi")]
public class OwsApiController(IOwsRepoService owsRepoService,
	IOperaAvailabilityService operaAvailabilityService,
	CustomAvailabilityService customAvailabilityService) : Controller
{


	[HttpGet]
	[Route("OwsConfigMaps")]
	public async Task<IActionResult> OwsConfigMaps()
	{
		var owsConfig = await owsRepoService.GetOwsConfigAsync();
		
		if (owsConfig != null)
		{
			return Ok(new ObjectVm
			{
				Data = owsConfig.SchemeMap
			});
		}

		return Ok(new ObjectVm
		{
			Data = new List<OwsPaymentCodeMap>()
		});
	}

	[HttpPost]
	[Route("OwsConfigMaps")]
	public async Task<IActionResult> OwsConfigMaps([FromBody] OwsConfigVm data)
	{
		try
		{
			await Task.Delay(0);

			var owsConfig = await owsRepoService.GetOwsConfigAsync();
			owsConfig.SchemeMap = data.SchemaMaps;

			await owsRepoService.SaveOwsConfigAsync(owsConfig);

			return Ok(new ObjectVm
			{
				Data = owsConfig.SchemeMap
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


	[Route("OwsConfigEditTriggers")]
	public async Task<IActionResult> OwsConfigEditTriggers()
	{
		var owsConfig = await owsRepoService.GetOwsConfigAsync();

		if (owsConfig != null)
		{
			return Ok(new ObjectVm
			{
				Data = owsConfig.CriticalErrorTriggers
			});
		}

		return Ok(new ObjectVm
		{
			Data = new List<OwsPaymentCodeMap>()
		});
	}


	[HttpPost]
	[Route("OwsConfigEditTriggers")]
	public async Task<IActionResult> OwsConfigEditTriggers([FromBody] OwsConfigVm data)
	{
		try
		{
			await Task.Delay(0);

			var owsConfig = await owsRepoService.GetOwsConfigAsync();
			owsConfig.CriticalErrorTriggers = data.CriticalErrorTriggers;

			await owsRepoService.SaveOwsConfigAsync(owsConfig);

			var owsConfig2 = await owsRepoService.GetOwsConfigAsync();

			return Ok(new ObjectVm
			{
				Data = owsConfig2.CriticalErrorTriggers
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
	[Route("Availability")]
	public async Task<IActionResult> Availability(RawAvailabilityReq req)
	{
		try
		{
			req.Depart = req.Arrive.AddDays(req.Nights);
			// AdminSiteAuth.VerifyUserForApi();
			
			var x = await LoadAvailability(req.DataType, req.HotelCode, req.Arrive, req.Nights, req.Adults,
				req.Children, req.Sort, req.QualifyingIdType, req.QualifyingIdValue);

			return Ok(x);
		}
		catch (InvalidCredentialException)
		{
			return NotFound();
		}
		catch (Exception ex)
		{
			return Ok(new ObjectVm { Success = false, ErrorMessage = ex.Message });
		}
	}

	private async Task GetAvailability(string hotelCode, DateTime arrive, int nights, int adults, int children, string sort, string qualifyingType, string qualifyingCode)
	{
		var req = new OwsAvailabilityRequest
		{
			AvailabilityMode = AvailabilityMode.Standard,
			CustomerType = CustomerTypes.PublicBooking,
			Adults = adults,
			Children = children,
			Infants = 0,
			Arrive = arrive,
			Depart = arrive.AddDays(nights)

		};
		var result = await operaAvailabilityService.GetGeneralAvailabilityAsync(req);
	}
	
	private async Task<AvailabilityVm> LoadAvailability( string dataType, string hotelCode, DateTime arrive, int nights,
		  int adults, int children, string sort, string qualifyingType, string qualifyingCode)
	{
		var vm = new AvailabilityVm();

		try
		{
			var today = DateTime.Now.Date;
			var depart = arrive.AddDays(nights);

			vm.Arrive = arrive;
			vm.Depart = depart;

			

			switch (dataType)
			{
				case "RoomTypes":
					var roomTypes = await customAvailabilityService.RunAvailabilityRoomTypes(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);
					vm.ListRoomTypes = roomTypes.OrderBy(r => r.RoomTypeCode).ToList();
					break;
				case "RatePlans":
					var ratePlans = await customAvailabilityService.RunAvailabilityRatePlans(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);
					vm.ListRatePlans = ratePlans.OrderBy(r => r.RatePlanCode).ToList();
					break;

				case "RoomRates":
				default:
					var list = await customAvailabilityService.RunAvailability(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);

					vm.List = list.OrderBy(r => r.RoomTypeCode).ThenBy(r => r.RatePlanCode).ThenBy(r => r.Rate).ToList();
					switch (sort.ToLower())
					{
						case "roomtype":
							vm.List = list.OrderBy(r => r.RoomTypeCode).ThenBy(r => r.RatePlanCode).ThenBy(r => r.Rate).ToList();
							break;
						case "rateplan":
							vm.List = list.OrderBy(r => r.RatePlanCode).ThenBy(r => r.RoomTypeCode).ThenBy(r => r.Rate).ToList();
							break;
						case "price":
							vm.List = list.OrderBy(r => r.Rate).ThenBy(r => r.RoomTypeCode).ThenBy(r => r.RoomTypeCode).ToList();
							break;
					}
					break;
			}


		}
		catch (Exception ex)
		{
			vm.ErrorMessage = ex.Message;
			vm.Success = false;
		}

		return vm;
	}

}