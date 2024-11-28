﻿using Microsoft.AspNetCore.Mvc;
using Msh.Admin.Models;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

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
	public async Task<IActionResult> HotelsPublish([FromBody] NotesSaveData saveData)
	{
		try
		{
			var userId = userService.GetUserId();

			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await hotelRepository.Publish(userId, saveData.Notes);

			if (!result)
			{
				return GetFail("The publish operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"Hotels Publish: {ex.Message}");
		}
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
		try
		{
			var list = await hotelRepository.ArchivedList();

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
			return GetFail($"Hotels Archive List: {ex.Message}");
		}
	}

	/// <summary>
	/// Perform an archive save. Gets the current edited data from Config,
	/// and stores it in ConfigArchive with the archiveCode appended
	/// </summary>
	/// <param name="archiveCode"></param>
	/// <param name="saveData"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("HotelsArchive/{archiveCode}")]
	public async Task<IActionResult> HotelsArchive(string archiveCode, [FromBody] NotesSaveData saveData)
	{
		try
		{
			var userId = userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await hotelRepository.Archive(archiveCode, userId, saveData.Notes);
			if (!result)
			{
				return GetFail("The publish operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"Hotels Archive {archiveCode}: {ex.Message}");
		}
	}

	[HttpPost]
	[Route("HotelsLock")]
	public async Task<IActionResult> HotelsLock([FromBody] ApiInput input)
	{
		try
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
		catch (Exception ex)
		{
			return GetFail($"Hotels Lock: {ex.Message}");
		}
	}

	[HttpPost]
	[Route("HotelsLoad")]
	public async Task<IActionResult> HotelsLoad([FromBody] HotelBaseVm data)
	{
		try
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
		catch (Exception ex)
		{
			return GetFail($"Hotels Load {data.Code}: {ex.Message}");
		}
	}
}