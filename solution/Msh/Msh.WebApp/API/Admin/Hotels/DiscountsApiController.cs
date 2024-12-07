using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

[ApiController]
[Route("api/discountsapi")]
public partial class DiscountsApiController : PrivateApiController
{
	private readonly IConfigRepository _configRepository;
	private readonly IDiscountRepository _discountRepository;
	private readonly IUserService _userService;

	public DiscountsApiController(IConfigRepository configRepository,
		IHotelRepository hotelRepository,
		IDiscountRepository discountRepository,
		IUserService userService) : base(hotelRepository)
	{
		_configRepository = configRepository;
		_discountRepository = discountRepository;
		_userService = userService;
	}

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

			var discounts = await _discountRepository.GetData(input.HotelCode);

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

				var newDiscounts = await _discountRepository.GetData(input.NewHotelCode);
				if (newDiscounts.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newDiscounts.Add(newDiscount);
				await _discountRepository.Save(newDiscounts, input.NewHotelCode);
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

			var hotels = await HotelRepository.GetData();

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

			var srcItems = await _discountRepository.GetData(input.HotelCode);
			var dstItems = await _discountRepository.GetData(input.NewHotelCode);

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

			await _discountRepository.Save(dstItems, input.NewHotelCode);

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
			var hotels = await HotelRepository.GetData();

			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await _discountRepository.GetData(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.Code)))
				{
					items.RemoveAt(i);
				}
			}

			await _discountRepository.Save(items, input.HotelCode);


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
			var hotels = await HotelRepository.GetData();

			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcItems = await _discountRepository.GetData(input.HotelCode);

			await _discountRepository.Save(srcItems.OrderBy(e => e.Code).ToList(), input.HotelCode);

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
		var items = await _discountRepository.GetData(hotelCode);
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

			var items = await _discountRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].OfferDates = data.Dates;
				await _discountRepository.Save(items, data.HotelCode);
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
		var items = await _discountRepository.GetData(hotelCode);
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
			
			var items = await _discountRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].BookDates = data.Dates;
				await _discountRepository.Save(items, data.HotelCode);
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
			

			var items = await _discountRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].EnabledHotelPlans = data.CodeList;
				await _discountRepository.Save(items, data.HotelCode);
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

			var items = await _discountRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].DisabledHotelPlans = data.CodeList;
				await _discountRepository.Save(items, data.HotelCode);
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
	[Route("DiscountErrors")]
	public async Task<IActionResult> DiscountErrors(string code, string hotelCode)
	{
		var items = await _discountRepository.GetData(hotelCode);
		var item = items.FirstOrDefault(h => h.Code == code);
		if (item == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Errors not found for hotel {hotelCode} and {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Errors = item.DiscountErrors,
				Types = DiscountErrorsHelper.GetErrorTypes().Select(e => new SelectItemVm { Value = e, Text = e }).ToList()
			}
		});
	}

	[HttpPost]
	[Route("DiscountErrors")]
	public async Task<IActionResult> DiscountErrors([FromBody] DiscountErrorsVm data)
	{
		try
		{

			var items = await _discountRepository.GetData(data.HotelCode);
			var index = items.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				items[index].DiscountErrors = data.Errors;
				await _discountRepository.Save(items, data.HotelCode);
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