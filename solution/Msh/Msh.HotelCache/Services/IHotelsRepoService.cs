using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Models.Specials;

namespace Msh.HotelCache.Services;

public interface IHotelsRepoService
{
	Task<List<Hotel>> GetHotelsAsync();

	Task SaveHotelsAsync(List<Hotel> hotels);

	Task<List<RoomType>> GetRoomTypesAsync(string hotelCode);

	Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode);

	Task<List<RoomRatePlan>> GetRatePlansAsync(string hotelCode);

	Task SaveRatePlansAsync(List<RoomRatePlan> ratePlans, string hotelCode);

	Task<List<Extra>> GetExtrasAsync(string hotelCode);

	Task SaveExtrasAsync(List<Extra> extras, string hotelCode);

	Task<List<Special>> GetSpecialsAsync(string hotelCode);

	Task SaveSpecialsAsync(List<Special> specials, string hotelCode);

	Task<List<TestModel>> GetTestModelsAsync();

	Task SaveTestModelsAsync(List<TestModel> testModels);

	Task<List<DiscountCode>> GetDiscountCodesAsync(string hotelCode);

	Task SaveDiscountCodesAsync(List<DiscountCode> discountCodes, string hotelCode);
}