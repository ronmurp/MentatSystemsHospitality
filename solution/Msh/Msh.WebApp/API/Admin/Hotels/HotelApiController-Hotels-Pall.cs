using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.Configuration;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

public partial class HotelApiController
{
	/// <summary>
	/// Publishes the Hotels list, copying the hotel list from Config to ConfigPub table
	/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
	/// must not be locked.
	/// </summary>
	/// <returns></returns>
	[HttpPost]
	[Route("HotelsPublish")]
	public async Task<IActionResult> HotelsPublish()
	{
		var userId = userService.GetUserId();

		if (string.IsNullOrEmpty(userId))
		{
			return GetFail("You must be signed-in to perform this action.");
		}

		var result = await hotelRepository.Publish(userId);

		if (!result)
		{
			return GetFail("The publish operation failed. The record may be locked.");
		}

		return Ok(new ObjectVm());
	}

	/// <summary>
	/// Returns a select list so that the user can select one of the archive records from ConfigArchive,
	/// or the published data from ConfigPub
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route("HotelsArchiveSelectList")]
	public async Task<IActionResult> HotelsArchiveSelectList()
	{
		var list = await hotelRepository.ArchivedList();

		var selectList = list.OrderBy(x => x.ConfigType).Select(x => new AdminSelectItem
		{
			Value = x.ConfigType,
			Text = x.ConfigType

		}).ToList();

		selectList.Insert(0, new AdminSelectItem
		{
			Value = "Pub",
			Text = "Published"

		});

		return Ok(new ObjectVm
		{
			Data = selectList
		});
	}

	/// <summary>
	/// Perform an archive save. Gets the current edited data from Config,
	/// and stores it in ConfigArchive with the archiveCode appended
	/// </summary>
	/// <param name="obj">unused</param>
	/// <param name="archiveCode"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("HotelsArchive/{archiveCode}")]
	public async Task<IActionResult> HotelsArchive(string archiveCode)
	{
		var userId = userService.GetUserId();
		if (string.IsNullOrEmpty(userId))
		{
			return GetFail("You must be signed-in to perform this action.");
		}
		var result = await hotelRepository.Archive(archiveCode, userId);
		if (!result)
		{
			return GetFail("The publish operation failed. The record may be locked.");
		}

		return Ok(new ObjectVm());
	}

	[HttpPost]
	[Route("HotelsLock")]
	public async Task<IActionResult> HotelsLock([FromBody] ApiInput input)
	{
		var userId = userService.GetUserId();
		if (string.IsNullOrEmpty(userId))
		{
			return GetFail("You must be signed-in to perform this action.");
		}

		switch (input.Code)
		{
			case "Pub":
				var resultP = await hotelRepository.LockPublished(input.IsTrue, userId);
				if (!resultP)
				{
					return GetFail("The publish operation failed. The record may be locked.");
				}
				break;

			default:
				var resultA = await hotelRepository.LockArchived(input.Code, input.IsTrue, userId);
				if (!resultA)
				{
					return GetFail("The publish operation failed. The record may be locked.");
				}
				break;
		}
		

		return Ok(new ObjectVm());
	}

	[HttpPost]
	[Route("HotelsLoad")]
	public async Task<IActionResult> HotelsLoad([FromBody] HotelBaseVm data)
	{
		var userId = userService.GetUserId();
		if (string.IsNullOrEmpty(userId))
		{
			return GetFail("You must be signed-in to perform this action.");
		}

		var archiveCode = data?.Code ?? string.Empty;

		switch (archiveCode)
		{
			case "Pub":
				var hotelsPub = await hotelRepository.Published();
				await hotelRepository.Save(hotelsPub);
				break;

			default:
				var hotelsArch = await hotelRepository.Archived(archiveCode);
				await hotelRepository.Save(hotelsArch);
				break;
		}

		return Ok(new ObjectVm());
	}
}

public partial class HotelApiController
{
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
		var hotels = await hotelRepository.GetData();
		if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
		{
			return (GetFail($"The hotel does not exist: {hotelCode}"), false);
		}

		return (Ok(new ObjectVm()), true);
	}

	
}