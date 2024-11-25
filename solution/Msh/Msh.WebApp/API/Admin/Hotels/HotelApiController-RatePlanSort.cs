using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.RatePlans;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.API.Admin.Hotels;

public partial class HotelApiController
{
	
	/// <summary>
	/// Add the current rate plans - add any missing, remove any extras
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("RatePlanSortAdd")]
	public async Task<IActionResult> RatePlanSortAdd(ApiInput input)
	{
		try
		{
			var ratePlans = await ratePlanRepository.GetData(input.HotelCode);

			var sortList = await ratePlanSortRepository.GetData(input.HotelCode);
			var index = sortList.Count;

			foreach (var rp in ratePlans)
			{
				if (sortList.All(r => r.Code != rp.RatePlanCode))
				{
					sortList.Add(new RatePlanSort
					{
						Code = rp.RatePlanCode,
						Order = index++
					});
				}
			}

			for (var i = sortList.Count - 1; i >= 0; i--)
			{
				if (sortList[i].Code == "XXX")
				{
					var code = sortList[i].Code;
				}
				if (ratePlans.All(r => r.RatePlanCode != sortList[i].Code))
				{
					sortList.RemoveAt(i);
				}
			}

			await ratePlanSortRepository.Save(sortList, input.HotelCode);

			return Ok(new ObjectVm
			{
				Data = new
				{
					List = sortList
				}
			});
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}


	[HttpPost]
	[Route("RatePlanSortPublish/{hotelCode}")]
	public async Task<IActionResult> RatePlanSortPublish(string hotelCode)
	{
		try
		{
			var userId = userService.GetUserId();

			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}


			var result = await ratePlanSortRepository.Publish(hotelCode, userId);

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
			var list = await ratePlanSortRepository.ArchivedList(hotelCode);

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
	public async Task<IActionResult> RatePlanSortArchive(string hotelCode, string archiveCode)
	{
		try
		{
			var userId = userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var result = await ratePlanSortRepository.Archive(hotelCode, archiveCode, userId);
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
			var userId = userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}


			switch (input.Code)
			{
				case "Pub":

					var resultP = await ratePlanSortRepository.LockPublished(hotelCode, input.IsTrue, userId);
					if (!resultP)
					{
						return GetFail("The publish operation failed. The record may be locked.");
					}

					break;

				default:
					var resultA =
						await ratePlanSortRepository.LockArchived(hotelCode, input.Code, input.IsTrue, userId);
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
			var userId = userService.GetUserId();
			if (string.IsNullOrEmpty(userId))
			{
				return GetFail("You must be signed-in to perform this action.");
			}

			var archiveCode = data?.Code ?? string.Empty;

			switch (archiveCode)
			{
				case "Pub":
					var recordsPub = await ratePlanSortRepository.Published(hotelCode);
					await ratePlanSortRepository.Save(recordsPub, hotelCode);
					break;

				default:
					var recordsArch = await ratePlanSortRepository.Archived(hotelCode, archiveCode);
					await ratePlanSortRepository.Save(recordsArch, hotelCode);
					break;
			}

			return Ok(new ObjectVm());
		}
		catch (Exception ex)
		{
			return GetFail($"RatePlanSortLoad {hotelCode} {data.Code}: {ex.Message}");
		}
	}
}
