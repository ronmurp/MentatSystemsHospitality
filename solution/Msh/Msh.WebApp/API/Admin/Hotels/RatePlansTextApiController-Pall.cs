using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels
{
	[ApiController]
	[Route("api/rateplantextapi")]
	public partial class RatePlanTextApiController : PrivateApiController
	{
		private const string ModelName = "RatePlanText";

		private readonly IUserService _userService;
		private readonly IRatePlanTextRepository _ratePlanTextRepository;

		public RatePlanTextApiController(IHotelRepository hotelRepository,
			IUserService userService,
			IRatePlanTextRepository ratePlanTextRepository) : base(hotelRepository)
		{
			_userService = userService;
			_ratePlanTextRepository = ratePlanTextRepository;
		}

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
				var userId = _userService.GetUserId();

				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}


				var result = await _ratePlanTextRepository.Publish(hotelCode, userId, saveData.Notes);

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
				var list = await _ratePlanTextRepository.ArchivedList(hotelCode);

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
				var userId = _userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await _ratePlanTextRepository.Archive(hotelCode, archiveCode, userId, saveData.Notes);
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
				var userId = _userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}


				switch (input.Code)
				{
					case "Pub":

						var resultP = await _ratePlanTextRepository.LockPublished(hotelCode, input.IsTrue, userId);
						if (!resultP)
						{
							return GetFail("The publish operation failed. The record may be locked.");
						}

						break;

					default:
						var resultA =
							await _ratePlanTextRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
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
				var userId = _userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var archiveCode = data?.Code ?? string.Empty;

				switch (archiveCode)
				{
					case "Pub":
						var recordsPub = await _ratePlanTextRepository.Published(hotelCode);
						await _ratePlanTextRepository.Save(recordsPub, hotelCode);
						break;

					default:
						var recordsArch = await _ratePlanTextRepository.Archived(hotelCode, archiveCode);
						await _ratePlanTextRepository.Save(recordsArch, hotelCode);
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
				var userId = _userService.GetUserId();
				if (string.IsNullOrEmpty(userId))
				{
					return GetFail("You must be signed-in to perform this action.");
				}

				var result = await _ratePlanTextRepository.ArchiveDelete(hotelCode, archiveCode, userId);
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
