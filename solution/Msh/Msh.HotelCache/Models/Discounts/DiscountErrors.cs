using Msh.Common.Models;

namespace Msh.HotelCache.Models.Discounts;

public class DiscountErrors : ParameteriseContainer<DiscountErrors>
{
    public string InvalidCode { get; set; } = string.Empty;

    public string RequiresCopyPasteCode { get; set; } = "Copy-paste your unique code";

    public string NotApplied { get; set; } = string.Empty;

    public string SearchError { get; set; } = string.Empty;

    public string WrongHotel { get; set; } = string.Empty;
    public string SelectHotel { get; set; } = string.Empty;
    public string SelectAnyHotel { get; set; } = string.Empty;
    public string SelectAnotherHotel { get; set; } = string.Empty;

    public string NotAvailableDates { get; set; } = string.Empty;
    public string InvalidBookDates { get; set; } = string.Empty;

    public string InvalidRooms { get; set; } = string.Empty;
    public string InvalidNights { get; set; } = string.Empty;

    public string DisabledByRatePlan { get; set; } = string.Empty;
    public string EnterEmail { get; set; } = string.Empty;


    public string OneTimeInvalid { get; set; } = string.Empty;
    public string OneTimeExpired { get; set; } = string.Empty;
    public string OneTimeUsed { get; set; } = string.Empty;

    public string InvalidBooking { get; set; } = "Invalid Booking";
    public string InvalidBookingStatus { get; set; } = string.Empty;

    // Todo - Discount Error - RoomLimit - Not used
    public string RoomLimit { get; set; } = "The discount is not available for these dates.";
    public string InvalidDepartDate { get; set; } = "The offer is not available until after departure.";
}