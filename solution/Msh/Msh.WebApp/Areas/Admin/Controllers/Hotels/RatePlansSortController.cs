using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.Admin.Models.Const;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels
{

	[Authorize]
	[Area("Admin")]
	[Route(AdminRoutes.RatePlansSort)]
	public class RatePlansSortController(ILogger<HotelsController> logger,
		IHotelRepository hotelRepository,
		IRatePlanSortRepository ratePlanSortRepository,
		IRoomTypeRepository roomTypeRepository,
		IRatePlanRepository ratePlanRepository,
		IUserService userService) : BaseAdminController(hotelRepository)
	{

		private async Task<List<RatePlanSortVm>> TransformList(List<RatePlanSort> ratePlanSort, string hotelCode)
		{
			var ratePlans = await ratePlanRepository.GetData(hotelCode);
			var list = new List<RatePlanSortVm>();
			foreach (var rps in ratePlanSort.OrderBy(x => x.Order).ThenBy(x => x.Code))
			{
				var z = new RatePlanSortVm
				{
					Code = rps.Code,
					Order = rps.Order,
					Text = string.Empty
				};
				var rp = ratePlans.FirstOrDefault(y => y.RatePlanCode == rps.Code);
				if (rp != null)
				{
					z.Text = rp.Title ?? string.Empty;
				}
				list.Add(z);
			}

			return list;
		}

		[HttpGet]
		[Route("RatePlanSortList")]
		public async Task<IActionResult> RatePlanSortList([FromQuery] string hotelCode = "")
		{
			var vm = new RatePlanSortListVm
			{
				HotelCode = string.IsNullOrEmpty(hotelCode) ? string.Empty : hotelCode,
				HotelName = string.Empty
			};

			try
			{
				await Task.Delay(0);

				vm.Hotels = await HotelRepository.GetData();

				var hotel = string.IsNullOrEmpty(hotelCode)
					? vm.Hotels.FirstOrDefault()
					: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

				vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;
				vm.HotelName = hotel != null ? hotel.Name : string.Empty;

				var ratePlanSort = (await ratePlanSortRepository.GetData(vm.HotelCode)) ?? [];

				ratePlanSort = ratePlanSort.OrderBy(x => x.Order).ThenBy(x => x.Code).ToList();

				await ratePlanSortRepository.Save(ratePlanSort, hotelCode);

				vm.RatePlanSorts = await TransformList(ratePlanSort, vm.HotelCode);

				return View(vm);
			}
			catch (NullConfigException ex)
			{
				//if (!string.IsNullOrEmpty(vm.HotelCode))
				//{
				//	await ratePlanRepository.SaveMissingConfigAsync($"{ConstHotel.Cache.RoomTypes}-{vm.HotelCode}", new List<RoomType>());
				//}

				vm.ErrorMessage = $"No room types for hotel {vm.HotelCode}";

				return View(vm);
			}
			catch (Exception ex)
			{
				logger.LogError($"{ex.Message}");
				vm.ErrorMessage = $"Error for hotel {vm.HotelCode}. {ex.Message}";
				return View(vm);
			}
		}

		[HttpPost]
		[Route("RatePlanSortMove")]
		public async Task<IActionResult> RatePlanSortMove([FromBody] ApiInput input)
		{
			try
			{

				var hotelCode = input.HotelCode;
				var hotels = await HotelRepository.GetData();
				if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
				{
					return GetFail($"Invalid hotel code {hotelCode}");
				}

				var srcItems = await ratePlanSortRepository.GetData(hotelCode);
				srcItems = srcItems.OrderBy(x => x.Order).ThenBy(x => x.Code).ToList();

				var currentIndex = srcItems.FindIndex(item => item.Code.EqualsAnyCase(input.Code));
				var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
				var currentItem = srcItems[currentIndex];
				var swapItem = srcItems[swapIndex];
				srcItems[swapIndex] = currentItem;
				srcItems[currentIndex] = swapItem;

				for (var i = 0; i < srcItems.Count - 1; i++)
					srcItems[i].Order = i;

				await ratePlanSortRepository.Save(srcItems, hotelCode);


				var table = new RatePlanSortListVm
				{
					Hotels = hotels,
					HotelCode = hotelCode,
					RatePlanSorts = await TransformList(srcItems, hotelCode)
				};

				return PartialView("Hotels/_RatePlanSortTable", table);

			}
			catch (Exception ex)
			{
				return GetFail(ex.Message);
			}
		}


	}

}
