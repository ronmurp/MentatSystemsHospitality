namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// Type of discount errors
/// </summary>
public enum DiscountErrorType
{
	InvalidCode,
	RequiresCopyPasteCode,
	NotApplied,
	SearchError,
	WrongHotel,
	SelectHotel,
	SelectAnyHotel,
	SelectAnotherHotel,
	NotAvailableDates,
	InvalidBookDates,
	InvalidRooms,
	InvalidNights,
	DisabledByRatePlan,
	EnterEmail,
	OneTimeInvalid,
	OneTimeExpired,
	OneTimeUsed,
	InvalidBooking,
	InvalidBookingStatus,
	RoomLimit,
	InvalidDepartDate
}