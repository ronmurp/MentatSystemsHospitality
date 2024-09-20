using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Logger;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Config;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Areas.Admin.Models.Ows;
using System.Security.Authentication;

namespace Msh.WebApp.API.Ows;

[ApiController]
[Route("api/owsapi")]
public class OwsApiController(IOwsConfigService owsConfigService) : Controller
{

	[HttpGet]
	[Route("OwsConfigMaps")]
	public async Task<IActionResult> OwsConfigMaps()
	{
		var owsConfig = await owsConfigService.GetOwsConfigAsync();
		
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

			var owsConfig = await owsConfigService.GetOwsConfigAsync();
			owsConfig.SchemeMap = data.SchemaMaps;

			await owsConfigService.SaveOwsConfigAsync(owsConfig);

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
		var owsConfig = await owsConfigService.GetOwsConfigAsync();

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

			var owsConfig = await owsConfigService.GetOwsConfigAsync();
			owsConfig.CriticalErrorTriggers = data.CriticalErrorTriggers;

			await owsConfigService.SaveOwsConfigAsync(owsConfig);

			var owsConfig2 = await owsConfigService.GetOwsConfigAsync();

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
	public async Task<IActionResult> Availability(RawAvailabilityReq req)
	{
		try
		{
			req.Depart = req.Arrive.AddDays(req.Nights);
			// AdminSiteAuth.VerifyUserForApi();

			//return Ok(LoadAvailability(req.OwsSource, req.DataType, req.HotelCode, req.Arrive, req.Nights, req.Adults, req.Children, req.Sort, req.QualifyingIdType, req.QualifyingIdValue));
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

	//private AvailabilityVm LoadAvailability(string systemTypeStr, string runModeStr, string dataType, string hotelCode, DateTime arrive, int nights,
	//	  int adults, int children, string sort,
	//	  string qualifyingType, string qualifyingCode)
	//{
	//	var vm = new AvailabilityVm();

	//	try
	//	{
	//		var today = DateTime.Now.Date;
	//		var depart = arrive.AddDays(nights);




	//		vm.Arrive = arrive;
	//		vm.Depart = depart;


	//		var typedCacheService = new TypedCacheService();
	//		var logXmlService = new LogXmlService(typedCacheService);
	//		var sut = new CustomAvailabilityService(new OwsConfigService(new ConfigService(typedCacheService)), logXmlService);

	//		switch (dataType)
	//		{
	//			case "RoomTypes":
	//				var roomTypes = sut.RunAvailabilityRoomTypes(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);
	//				vm.ListRoomTypes = roomTypes.OrderBy(r => r.RoomTypeCode).ToList();
	//				break;
	//			case "RatePlans":
	//				var ratePlans = sut.RunAvailabilityRatePlans(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);
	//				vm.ListRatePlans = ratePlans.OrderBy(r => r.RatePlanCode).ToList();
	//				break;

	//			case "RoomRates":
	//			default:
	//				var list = sut.RunAvailability(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);

	//				vm.List = list.OrderBy(r => r.RoomTypeCode).ThenBy(r => r.RatePlanCode).ThenBy(r => r.Rate).ToList();
	//				switch (sort.ToLower())
	//				{
	//					case "roomtype":
	//						vm.List = list.OrderBy(r => r.RoomTypeCode).ThenBy(r => r.RatePlanCode).ThenBy(r => r.Rate).ToList();
	//						break;
	//					case "rateplan":
	//						vm.List = list.OrderBy(r => r.RatePlanCode).ThenBy(r => r.RoomTypeCode).ThenBy(r => r.Rate).ToList();
	//						break;
	//					case "price":
	//						vm.List = list.OrderBy(r => r.Rate).ThenBy(r => r.RoomTypeCode).ThenBy(r => r.RoomTypeCode).ToList();
	//						break;
	//				}
	//				break;
	//		}


	//	}
	//	catch (Exception ex)
	//	{
	//		vm.ErrorMessage = ex.Message;
	//		vm.Success = false;
	//	}

	//	return vm;
	//}

}