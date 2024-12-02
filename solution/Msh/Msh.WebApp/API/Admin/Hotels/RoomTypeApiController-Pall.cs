using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Hotels
{
	public partial class RoomTypeApiController
	{
		

		/// <summary>
		/// Publishes the Room Types list, copying the hotel list from Config to ConfigPub table
		/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
		/// must not be locked.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("RoomTypePublish/{hotelCode}")]
		public async Task<IActionResult> RoomTypePublish(string hotelCode)
		{
			try
			{
				var userId = _userService.GetUserId();

				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}


				var result = await _roomTypeRepository.Publish(hotelCode, userId);

				if (!result)
				{
					return GetFail("The publish operation failed. The record may be locked.");
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"{ModelName} Publish for {hotelCode}: {ex.Message}");
			}
		}

		/// <summary>
		/// Returns a select list so that the user can select one of the archive records from ConfigArchive,
		/// or the published data from ConfigPub 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("RoomTypeArchiveSelectList/{hotelCode}")]
		public async Task<IActionResult> RoomTypeArchiveSelectList(string hotelCode)
		{
			try
			{
				var list = await _roomTypeRepository.ArchivedList(hotelCode);

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
				return GetFail($"{ModelName} Archive {hotelCode}: {ex.Message}");
			}
		}

		/// <summary>
		/// Perform an archive save. Gets the current edited data from Config,
		/// and stores it in ConfigArchive with the archiveCode appended
		/// </summary>
		/// <param name="hotelCode"></param>
		/// <param name="archiveCode"></param>
		/// <param name="saveData"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("RoomTypeArchive/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> RoomTypeArchive(string hotelCode, string archiveCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = _userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await _roomTypeRepository.Archive(hotelCode, archiveCode, userId, saveData.Notes);
				if (!result)
				{
					return GetFail("The archive operation failed. The record may be locked.");
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"{ModelName} Archive {hotelCode} {archiveCode}: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("RoomTypeLock/{hotelCode}")]
		public async Task<IActionResult> RoomTypeLock([FromBody] ApiInput input, string hotelCode)
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

						var resultP = await _roomTypeRepository.LockPublished(hotelCode, input.IsTrue, userId);
						if (!resultP)
						{
							return GetFail("The publish operation failed. The record may be locked.");
						}

						break;

					default:
						var resultA =
							await _roomTypeRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
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
				return GetFail($"{ModelName} Lock: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("RoomTypeLoad/{hotelCode}")]
		public async Task<IActionResult> RoomTypeLoad([FromBody] HotelBaseVm data, string hotelCode)
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
						var recordsPub = await _roomTypeRepository.Published(hotelCode);
						await _roomTypeRepository.Save(recordsPub, hotelCode);
						break;

					default:
						var recordsArch = await _roomTypeRepository.Archived(hotelCode, archiveCode);
						await _roomTypeRepository.Save(recordsArch, hotelCode);
						break;
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"{ModelName} Load {hotelCode} {data.Code}: {ex.Message}");
			}
		}

		/// <summary>
		/// Perform an archive delete.
		/// </summary>
		/// <param name="hotelCode"></param>
		/// <param name="archiveCode"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("RoomTypeArchiveDelete/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> RoomTypeArchiveDelete(string hotelCode, string archiveCode)
		{
			try
			{
				var userId = _userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await _roomTypeRepository.ArchiveDelete(hotelCode, archiveCode, userId);
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
}
