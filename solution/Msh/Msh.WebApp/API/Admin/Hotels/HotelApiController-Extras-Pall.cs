using Microsoft.AspNetCore.Mvc;
using Msh.Admin.Imports;
using Msh.Admin.Models;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Hotels
{
	public partial class HotelApiController
	{

		/// <summary>
		/// Publishes the Rate Plans list, copying the hotel list from Config to ConfigPub table
		/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
		/// must not be locked.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("ExtrasPublish/{hotelCode}")]
		public async Task<IActionResult> ExtrasPublish(string hotelCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = userService.GetUserId();

				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}


				var result = await extraRepository.Publish(hotelCode, userId, saveData.Notes);

				if (!result)
				{
					return GetFail("The publish operation failed. The record may be locked.");
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"ExtrasPublish for {hotelCode}: {ex.Message}");
			}
		}

		/// <summary>
		/// Returns a select list so that the user can select one of the archive records from ConfigArchive,
		/// or the published data from ConfigPub 
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("ExtrasArchiveSelectList/{hotelCode}")]
		public async Task<IActionResult> ExtrasArchiveSelectList(string hotelCode)
		{
			try
			{
				var list = await extraRepository.ArchivedList(hotelCode);

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
				return GetFail($"RatePlans Archive {hotelCode}: {ex.Message}");
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
		[Route("ExtrasArchive/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> ExtrasArchive(string hotelCode, string archiveCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await extraRepository.Archive(hotelCode, archiveCode, userId, saveData.Notes);
				if (!result)
				{
					return GetFail("The archive operation failed. The record may be locked.");
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"ExtrasArchive {hotelCode} {archiveCode}: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("ExtrasLock/{hotelCode}")]
		public async Task<IActionResult> ExtrasLock([FromBody] ApiInput input, string hotelCode)
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

						var resultP = await extraRepository.LockPublished(hotelCode, input.IsTrue, userId);
						if (!resultP)
						{
							return GetFail("The lock operation failed. The record may be locked.");
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
				return GetFail($"RatePlansLock: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("ExtrasLoad/{hotelCode}")]
		public async Task<IActionResult> ExtrasLoad([FromBody] HotelBaseVm data, string hotelCode)
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
						var recordsPub = await extraRepository.Published(hotelCode);
						await extraRepository.Save(recordsPub, hotelCode);
						break;

					default:
						var recordsArch = await extraRepository.Archived(hotelCode, archiveCode);
						await extraRepository.Save(recordsArch, hotelCode);
						break;
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"ExtrasLoad {hotelCode} {data.Code}: {ex.Message}");
			}
		}

		[HttpPost]
		[Route("ExtrasImport/{hotelCode}")]
		public async Task<IActionResult> ExtrasImport([FromBody] HotelBaseVm data, string hotelCode)
		{
			try
			{
				var userId = userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\Extras\Extras-Live.xml";

				if (!System.IO.File.Exists(filename))
				{
					return GetFail($"ExtrasImport {hotelCode}: The import file does not exist.");
				}

				var list = await ImportExtrasHelper.ImportExtrasHotelXml(configRepository, filename, hotelCode);
				// var recordsPub = await extraRepository.Published(hotelCode);
				await extraRepository.Save(list, hotelCode);

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail($"ExtrasImport {hotelCode} {data.Code}: {ex.Message}");
			}
		}

	}
}
