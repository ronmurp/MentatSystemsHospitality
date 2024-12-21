using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels
{
	[ApiController]
	[Route("api/rateplantextapi")]
	public partial class RatePlanTextApiController(
		IHotelRepository hotelRepository,
		IUserService userService,
		IRatePlanTextRepository ratePlanTextRepository)
		: PrivateApiController(hotelRepository)
	{
		private const string ModelName = "RatePlanText";

		/// <summary>
		/// Publishes the Rate Plans list, copying the hotel list from Config to ConfigPub table
		/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
		/// must not be locked.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("RatePlanTextPublish/{hotelCode}")]
		public async Task<IActionResult> RatePlanTextPublish(string hotelCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = userService.GetUserId();

				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}


				var result = await ratePlanTextRepository.Publish(hotelCode, userId, saveData.Notes);

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
		[Route("RatePlanTextArchiveSelectList/{hotelCode}")]
		public async Task<IActionResult> RatePlanTextArchiveSelectList(string hotelCode)
		{
			try
			{
				var list = await ratePlanTextRepository.ArchivedList(hotelCode);

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
		[Route("RatePlanTextArchive/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> RatePlanTextArchive(string hotelCode, string archiveCode, [FromBody] NotesSaveData saveData)
		{
			try
			{
				var userId = userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await ratePlanTextRepository.Archive(hotelCode, archiveCode, userId, saveData.Notes);
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
		[Route("RatePlanTextLock/{hotelCode}")]
		public async Task<IActionResult> RatePlanTextLock([FromBody] ApiInput input, string hotelCode)
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

						var resultP = await ratePlanTextRepository.LockPublished(hotelCode, input.IsTrue, userId);
						if (!resultP)
						{
							return GetFail("The publish operation failed. The record may be locked.");
						}

						break;

					default:
						var resultA =
							await ratePlanTextRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
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
		[Route("RatePlanTextLoad/{hotelCode}")]
		public async Task<IActionResult> RatePlanTextLoad([FromBody] HotelBaseVm data, string hotelCode)
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
						var recordsPub = await ratePlanTextRepository.Published(hotelCode);
						await ratePlanTextRepository.Save(recordsPub, hotelCode);
						break;

					default:
						var recordsArch = await ratePlanTextRepository.Archived(hotelCode, archiveCode);
						await ratePlanTextRepository.Save(recordsArch, hotelCode);
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
		[Route("RatePlanTextArchiveDelete/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> RatePlanTextArchiveDelete(string hotelCode, string archiveCode)
		{
			try
			{
				var userId = userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await ratePlanTextRepository.ArchiveDelete(hotelCode, archiveCode, userId);
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

		[HttpPost]
		[Route("RatePlanTextCopy")]
		public async Task<IActionResult> RatePlanTextCopy(ApiInput input)
		{
			try
			{
				if (SameCodes(input))
				{
					return GetFail("At least one code must change");
				}

				var ratePlans = await ratePlanTextRepository.GetData(input.HotelCode);
				var ratePlan = ratePlans.FirstOrDefault(h => h.Id == input.Code);
				if (ratePlan != null)
				{


					var newRatePlan = ratePlan.Adapt(ratePlan);

					newRatePlan.Id = input.NewCode;

					var result = await CheckHotel(input.NewHotelCode);
					if (!result.success)
					{
						return GetFail("The hotel does not exist.");
					}

					var newRatePlans = await ratePlanTextRepository.GetData(input.NewHotelCode);
					if (newRatePlans.Any(c => c.Id.EqualsAnyCase(newRatePlan.Id)))
					{
						return GetFail("The code already exists.");
					}

					newRatePlans.Add(newRatePlan);
					await ratePlanTextRepository.Save(newRatePlans, input.NewHotelCode);
				}

				return Ok(new ObjectVm());
			}
			catch (Exception ex)
			{
				return GetFail(ex.Message);
			}
		}

		[HttpPost]
		[Route("RatePlanTextCopyBulk")]
		public async Task<IActionResult> RatePlanTextCopyBulk(ApiInput input)
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
				var newList = new List<RatePlanText>();

				var srcItems = await ratePlanTextRepository.GetData(input.HotelCode);
				var dstItems = await ratePlanTextRepository.GetData(input.NewHotelCode);

				foreach (var code in input.CodeList)
				{
					var extra = srcItems.FirstOrDefault(h => h.Id == code);
					if (extra != null)
					{
						if (dstItems.Any(e => e.Id == extra.Id))
						{
							// Already exists
							missingList.Add(extra.Id);
							continue;
						}
						newList.Add(extra);
					}
				}

				dstItems.AddRange(newList);

				await ratePlanTextRepository.Save(dstItems, input.NewHotelCode);

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
		[Route("RatePlanTextDeleteBulk")]
		public async Task<IActionResult> RatePlanTextDeleteBulk(ApiInput input)
		{
			try
			{
				var hotels = await HotelRepository.GetData();
				if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
				{
					return GetFail($"Invalid source hotel code {input.HotelCode}");
				}

				var items = await ratePlanTextRepository.GetData(input.HotelCode);

				for (var i = items.Count - 1; i >= 0; i--)
				{
					var item = items[i];
					if (input.CodeList.Any(c => c.EqualsAnyCase(item.Id)))
					{
						items.RemoveAt(i);
					}
				}

				await ratePlanTextRepository.Save(items, input.HotelCode);



				return Ok(new ObjectVm());

			}
			catch (Exception ex)
			{
				return GetFail(ex.Message);
			}
		}
	}
}
