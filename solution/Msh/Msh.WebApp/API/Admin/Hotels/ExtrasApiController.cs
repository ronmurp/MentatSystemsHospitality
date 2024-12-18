using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Admin.Models.Const;
using Msh.Common.Data;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

[ApiController]
[Route(AdminRoutes.ExtrasApi)]
public partial class ExtrasApiController : PrivateApiController
{
	private const string ModelName = "Extras";

	private readonly IConfigRepository _configRepository;
	private readonly IExtraRepository _extraRepository;
	private readonly IUserService _userService;

	public ExtrasApiController(IConfigRepository configRepository,
		IHotelRepository hotelRepository,
		IExtraRepository extraRepository,
		IUserService userService) : base(hotelRepository)
	{
		_configRepository = configRepository;
		_extraRepository = extraRepository;
		_userService = userService;
	}

	[HttpPost]
	[Route("ExtraDelete")]
	public async Task<IActionResult> ExtraDelete(ApiInput input)
	{
		try
		{
			var extras = await _extraRepository.GetData(input.HotelCode);
			var extra = extras.FirstOrDefault(h => h.Code == input.Code);
			if (extra != null)
			{
				extras.Remove(extra);
				await _extraRepository.Save(extras, input.HotelCode);
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
	[Route("ExtraCopy")]
	public async Task<IActionResult> ExtraCopy(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var extras = await _extraRepository.GetData(input.HotelCode);
			var extra = extras.FirstOrDefault(h => h.Code == input.Code);
			if (extra != null)
			{
				var newExtra = extra.Adapt(extra);
				newExtra.Code = input.NewCode;

				var result = await CheckHotel(input.NewHotelCode);
				if (!result.success)
				{
					return result.fail;
				}

				var newExtras = await _extraRepository.GetData(input.NewHotelCode);
				if (newExtras.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newExtras.Add(newExtra);
				await _extraRepository.Save(newExtras, input.NewHotelCode);
			}

			return Ok(new ObjectVm());
			
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("ExtraCopyBulk")]
	public async Task<IActionResult> ExtraCopyBulk(ApiInput input)
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
			var newList = new List<Extra>();

			var srcExtras = await _extraRepository.GetData(input.HotelCode);
			var dstExtras = await _extraRepository.GetData(input.NewHotelCode);

			foreach (var code in input.CodeList)
			{
				var extra = srcExtras.FirstOrDefault(h => h.Code == code);
				if (extra != null)
				{
					if (dstExtras.Any(e => e.Code == extra.Code))
					{
						// Already exists
						missingList.Add(extra.Code);
						continue;
					}
					newList.Add(extra);
				}
			}

			dstExtras.AddRange(newList);

			await _extraRepository.Save(dstExtras, input.NewHotelCode);

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
	[Route("ExtraDeleteBulk")]
	public async Task<IActionResult> ExtraDeleteBulk(ApiInput input)
	{
		try
		{
			var hotels = await HotelRepository.GetData();

			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await _extraRepository.GetData(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.Code)))
				{
					items.RemoveAt(i);
				}
			}

			await _extraRepository.Save(items, input.HotelCode);

			

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("ExtrasSort")]
	public async Task<IActionResult> ExtrasSort(ApiInput input)
	{
		try
		{
			var hotelCode = input.HotelCode;
			var hotels = await HotelRepository.GetData();

			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}
			
			var srcExtras = await _extraRepository.GetData(hotelCode);

			await _extraRepository.Save(srcExtras.OrderBy(e => e.Code).ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}



	[HttpGet]
	[Route("ExtraDates")]
	public async Task<IActionResult> ExtraDates(string code, string hotelCode)
	{
		var extras = await _extraRepository.GetData(hotelCode);
		var extra = extras.FirstOrDefault(h => h.Code == code);
		if (extra == null)
		{
			return Ok(new ObjectVm
			{
				Success = true,
				UserErrorMessage = $"Dates not found for hotel {hotelCode} and extra {code}"
			});
		}

		return Ok(new ObjectVm
		{
			Data = new
			{
				Dates = extra.ItemDates,
				MinDate = DateOnly.FromDateTime(DateTime.Now)
			}
		});
	}

	[HttpPost]
	[Route("ExtraDates")]
	public async Task<IActionResult> ExtraDates([FromBody] ItemDatesVm data)
	{
		try
		{
			await Task.Delay(0);

			var extras = await _extraRepository.GetData(data.HotelCode);
			var index = extras.FindIndex(h => h.Code == data.Code);

			if (index >= 0)
			{
				extras[index].ItemDates = data.Dates;
				await _extraRepository.Save(extras, data.HotelCode);
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