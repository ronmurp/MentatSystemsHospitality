using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Hotels
{
	public partial class HotelApiController
	{

		/// <summary>
		/// Publishes the Hotels list, copying the hotel list from Config to ConfigPub table
		/// with the Hotel configType. The user must be signed in, and the Published record in ConfigPub
		/// must not be locked.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("DiscountsPublish/{hotelCode}")]
		public async Task<IActionResult> DiscountsPublish(string hotelCode)
		{
			var userId = userService.GetUserId();

			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			
			var result = await discountRepository.Publish(hotelCode, userId);

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
		[Route("DiscountsArchiveSelectList/{hotelCode}")]
		public async Task<IActionResult> DiscountsArchiveSelectList(string hotelCode)
		{
			var list = await discountRepository.ArchivedList(hotelCode);

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
		/// <param name="hotelCode"></param>
		/// <param name="archiveCode"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("DiscountsArchive/{hotelCode}/{archiveCode}")]
		public async Task<IActionResult> DiscountsArchive(string hotelCode, string archiveCode)
		{
			var userId = userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}
			
			var result = await discountRepository.Archive(hotelCode, archiveCode, userId);
			if (!result)
			{
				return GetFail("The archive operation failed. The record may be locked.");
			}

			return Ok(new ObjectVm());
		}

		[HttpPost]
		[Route("DiscountsLock/{hotelCode}")]
		public async Task<IActionResult> DiscountsLock([FromBody] ApiInput input, string hotelCode)
		{
			var userId = userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}


			switch (input.Code)
			{
				case "Pub":
					
					var resultP = await discountRepository.LockPublished(hotelCode, input.IsTrue, userId);
					if (!resultP)
					{
						return GetFail("The publish operation failed. The record may be locked.");
					}
					break;

				default:
					var resultA = await discountRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
					if (!resultA)
					{
						return GetFail("The archive operation failed. The record may be locked.");
					}
					break;
			}


			return Ok(new ObjectVm());
		}

		[HttpPost]
		[Route("DiscountsLoad/{hotelCode}")]
		public async Task<IActionResult> DiscountsLoad([FromBody] HotelBaseVm data, string hotelCode)
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
					var recordsPub = await discountRepository.Published(hotelCode);
					await discountRepository.Save(recordsPub, hotelCode);
					break;

				default:
					var recordsArch = await discountRepository.Archived(hotelCode, archiveCode);
					await discountRepository.Save(recordsArch, hotelCode);
					break;
			}

			return Ok(new ObjectVm());
		}

	}
}
