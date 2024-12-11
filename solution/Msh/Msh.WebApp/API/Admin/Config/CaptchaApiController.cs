using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Data;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

[ApiController]
[Route("api/captchaapi")]
public class CaptchaApiController : PrivateApiController
{
	private const string ModelName = "Captcha";

	private readonly IConfigRepository _configRepository;
	private readonly ICaptchaConfigRepoService _captchaConfigRepoService;
	private readonly IUserService _userService;

	public CaptchaApiController(IConfigRepository configRepository,
		IHotelRepository hotelRepository,
		ICaptchaConfigRepoService captchaConfigRepoService,
		IUserService userService) : base(hotelRepository)
	{
		_configRepository = configRepository;
		_captchaConfigRepoService = captchaConfigRepoService;

		_userService = userService;
	}

	/// <summary>
	/// Publishes the Rate Plans list, copying the hotel list from Config to ConfigPub table
	/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
	/// must not be locked.
	/// </summary>
	/// <returns></returns>
	[HttpPost]
	[Route("CaptchaPublish/{hotelCode}")]
	public async Task<IActionResult> CaptchaPublish(string hotelCode, [FromBody] NotesSaveData saveData)
	{
		try
		{
			var userId = _userService.GetUserId();

			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await _captchaConfigRepoService.Publish(userId, saveData.Notes);

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
	[Route("CaptchaArchiveSelectList")]
	public async Task<IActionResult> CaptchaArchiveSelectList()
	{
		try
		{
			var list = await _captchaConfigRepoService.ArchivedList();

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
			return GetFail($"{ModelName} Archive: {ex.Message}");
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
	[Route("CaptchaArchive/{hotelCode}/{archiveCode}")]
	public async Task<IActionResult> CaptchaArchive(string hotelCode, string archiveCode, [FromBody] NotesSaveData saveData)
	{
		try
		{
			var userId = _userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await _captchaConfigRepoService.Archive(archiveCode, userId, saveData.Notes);
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

	/// <summary>
	/// Perform an archive delete.
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <param name="archiveCode"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("CaptchaArchiveDelete/{hotelCode}/{archiveCode}")]
	public async Task<IActionResult> CaptchaArchiveDelete(string archiveCode)
	{
		try
		{
			var userId = _userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await _captchaConfigRepoService.ArchiveDelete(archiveCode, userId);
			if (!result)
			{
				return GetFail("The archive delete operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"{ModelName} Archive {archiveCode}: {ex.Message}");
		}
	}


	[HttpPost]
	[Route("CaptchaLock/{hotelCode}")]
	public async Task<IActionResult> CaptchaLock([FromBody] ApiInput input, string hotelCode)
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

					var resultP = await _captchaConfigRepoService.LockPublished(input.IsTrue, userId);
					if (!resultP)
					{
						return GetFail("The lock operation failed. The record may be locked.");
					}

					break;

				default:
					var resultA =
						await _captchaConfigRepoService.LockArchived(input.Code, input.IsTrue, userId);
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
	[Route("CaptchaLoad/{hotelCode}")]
	public async Task<IActionResult> CaptchaLoad([FromBody] HotelBaseVm data, string hotelCode)
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
					var recordsPub = await _captchaConfigRepoService.Published();
					await _captchaConfigRepoService.Save(recordsPub);
					break;

				default:
					var recordsArch = await _captchaConfigRepoService.Archived(archiveCode);
					await _captchaConfigRepoService.Save(recordsArch);
					break;
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"{ModelName} Load {data.Code}: {ex.Message}");
		}
	}


}