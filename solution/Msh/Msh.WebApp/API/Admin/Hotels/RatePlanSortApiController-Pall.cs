using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Hotels;


public partial class RatePlansSortApiController
{

	[HttpPost]
	[Route("RatePlanSortPublish/{hotelCode}")]
	public async Task<IActionResult> RatePlanSortPublish(string hotelCode, [FromBody] NotesSaveData saveData)
	{
		try
		{
			var userId = _userService.GetUserId();

			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}


			var result = await _ratePlanSortRepository.Publish(hotelCode, userId, saveData.Notes);

			if (!result)
			{
				return GetFail("The publish operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"RatePlanSortPublish for {hotelCode}: {ex.Message}");
		}
	}

	[HttpGet]
	[Route("RatePlanSortArchiveSelectList/{hotelCode}")]
	public async Task<IActionResult> RatePlanSortArchiveSelectList(string hotelCode)
	{
		try
		{
			var list = await _ratePlanSortRepository.ArchivedList(hotelCode);

			list = list ?? [];

			var selectList = list.OrderBy(x => x.ConfigType).Select(x => new SelectItemVm
			{
				Value = x.ConfigType,
				Text = x.ConfigType

			}).ToList();

			selectList.Insert(0, new SelectItemVm
			{
				Value = "Pub",
				Text = "Published"

			});

			return Ok(new ObjectVm
			{
				Data = selectList
			});
		}
		catch (Exception ex)
		{
			return GetFail($"RatePlanSortArchive {hotelCode}: {ex.Message}");
		}
	}

	[HttpPost]
	[Route("RatePlanSortArchive/{hotelCode}/{archiveCode}")]
	public async Task<IActionResult> RatePlanSortArchive(string hotelCode, string archiveCode, [FromBody] NotesSaveData saveData)
	{
		try
		{
			var userId = _userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await _ratePlanSortRepository.Archive(hotelCode, archiveCode, userId, saveData.Notes);
			if (!result)
			{
				return GetFail("The archive operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"RatePlanSortArchive {hotelCode} {archiveCode}: {ex.Message}");
		}
	}

	[HttpPost]
	[Route("RatePlanSortLock/{hotelCode}")]
	public async Task<IActionResult> RatePlanSortLock([FromBody] ApiInput input, string hotelCode)
	{
		try
		{
			var userId = _userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}


			switch (input.Code)
			{
				case "Pub":

					var resultP = await _ratePlanSortRepository.LockPublished(hotelCode, input.IsTrue, userId);
					if (!resultP)
					{
						return GetFail("The publish operation failed. The record may be locked.");
					}

					break;

				default:
					var resultA =
						await _ratePlanSortRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
					if (!resultA)
					{
						return GetFail("The archive operation failed. The record may be locked.");
					}

					break;
			}


			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"RatePlanSortLock: {ex.Message}");
		}
	}

	[HttpPost]
	[Route("RatePlanSortLoad/{hotelCode}")]
	public async Task<IActionResult> RatePlanSortLoad([FromBody] HotelBaseVm data, string hotelCode)
	{
		try
		{
			var userId = _userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var archiveCode = data?.Code ?? string.Empty;

			switch (archiveCode)
			{
				case "Pub":
					var recordsPub = await _ratePlanSortRepository.Published(hotelCode);
					await _ratePlanSortRepository.Save(recordsPub, hotelCode);
					break;

				default:
					var recordsArch = await _ratePlanSortRepository.Archived(hotelCode, archiveCode);
					await _ratePlanSortRepository.Save(recordsArch, hotelCode);
					break;
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"RatePlanSortLoad {hotelCode} {data.Code}: {ex.Message}");
		}
	}

	/// <summary>
	/// Perform an archive delete.
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <param name="archiveCode"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("RatePlanSortArchiveDelete/{hotelCode}/{archiveCode}")]
	public async Task<IActionResult> RatePlanSortArchiveDelete(string hotelCode, string archiveCode)
	{
		try
		{
			var userId = _userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await _ratePlanSortRepository.ArchiveDelete(hotelCode, archiveCode, userId);
			if (!result)
			{
				return GetFail("The archive delete operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"{ModelName} Archive {hotelCode} {archiveCode}: {ex.Message}");
		}
	}

}