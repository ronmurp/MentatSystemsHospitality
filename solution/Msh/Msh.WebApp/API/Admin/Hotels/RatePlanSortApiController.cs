using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Msh.Common.Models;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

[ApiController]
[Route("api/rateplansortapi")]
public partial class RatePlanSortApiController : PrivateApiController
{
	private const string ModelName = "RatePlanSort";

	private readonly IRatePlanRepository _ratePlanRepository;
	private readonly IRatePlanSortRepository _ratePlanSortRepository;
	private readonly IUserService _userService;

	public RatePlanSortApiController(IHotelRepository hotelRepository,
		IRatePlanRepository ratePlanRepository,
		IRatePlanSortRepository ratePlanSortRepository,
		IUserService userService):base(hotelRepository)
	{
		_ratePlanRepository = ratePlanRepository;
		_ratePlanSortRepository = ratePlanSortRepository;
		_userService = userService;
	}
	
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
			var ratePlans = await _ratePlanRepository.GetData(input.HotelCode);

			var sortList = await _ratePlanSortRepository.GetData(input.HotelCode);
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

			await _ratePlanSortRepository.Save(sortList, input.HotelCode);

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

}