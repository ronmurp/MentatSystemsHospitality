using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Hotels
{
	public partial class RatePlanApiController
	{
		

		/// <summary>
		/// Publishes the Rate Plans list, copying the hotel list from Config to ConfigPub table
		/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
		/// must not be locked.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("RatePlansPublish/{hotelCode}")]
		public async Task<IActionResult> RatePlansPublish(string hotelCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = userService.GetUserId();

				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}


				var result = await ratePlanRepository.Publish(hotelCode, userId, saveData.Notes);

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
		[Route("RatePlansArchiveSelectList/{hotelCode}")]
		public async Task<IActionResult> RatePlansArchiveSelectList(string hotelCode)
		{
			try
			{
				var list = await ratePlanRepository.ArchivedList(hotelCode);

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
		[Route("RatePlansArchive/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> RatePlansArchive(string hotelCode, string archiveCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await ratePlanRepository.Archive(hotelCode, archiveCode, userId, saveData.Notes);
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
		[Route("RatePlansLock/{hotelCode}")]
		public async Task<IActionResult> RatePlansLock([FromBody] ApiInput input, string hotelCode)
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

						var resultP = await ratePlanRepository.LockPublished(hotelCode, input.IsTrue, userId);
						if (!resultP)
						{
							return GetFail("The publish operation failed. The record may be locked.");
						}

						break;

					default:
						var resultA =
							await ratePlanRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
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
		[Route("RatePlansLoad/{hotelCode}")]
		public async Task<IActionResult> RatePlansLoad([FromBody] HotelBaseVm data, string hotelCode)
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
						var recordsPub = await ratePlanRepository.Published(hotelCode);
						await ratePlanRepository.Save(recordsPub, hotelCode);
						break;

					default:
						var recordsArch = await ratePlanRepository.Archived(hotelCode, archiveCode);
						await ratePlanRepository.Save(recordsArch, hotelCode);
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
		[Route("RatePlansArchiveDelete/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> RatePlansArchiveDelete(string hotelCode, string archiveCode)
		{
			try
			{
				var userId = userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await ratePlanRepository.ArchiveDelete(hotelCode, archiveCode, userId);
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
