using System.ComponentModel.DataAnnotations;
using Msh.Common.Attributes;
using Msh.Common.Models;

namespace Msh.HotelCache.Models.Discounts;

public class DiscountErrors : ParameteriseContainer<DiscountErrors>
{
	[Display(Name = "Invalid Code")]
	[Info("")]
	public string? InvalidCode { get; set; } = string.Empty;

	[Display(Name = "Requires Copy-Paste Code")]
	[Info("")]
	public string? RequiresCopyPasteCode { get; set; } = "Copy-paste your unique code";

	[Display(Name = "Not Applied")]
	[Info("")]
	public string? NotApplied { get; set; } = string.Empty;

	[Display(Name = "Search Error")]
	[Info("")]
	public string? SearchError { get; set; } = string.Empty;

	[Display(Name = "Wrong Hotel")]
	[Info("")]
	public string? WrongHotel { get; set; } = "This discount is not available in this hotel.";

	[Display(Name = "Select Hotel")]
	[Info("")]
	public string? SelectHotel { get; set; } = string.Empty;

	[Display(Name = "Select Any Hotel")]
	[Info("")]
	public string? SelectAnyHotel { get; set; } = string.Empty;

	[Display(Name = "")]
	[Info("")]
	public string? SelectAnotherHotel { get; set; } = string.Empty;

	[Display(Name = "Select Another Hotel")]
	[Info("")]
	public string? NotAvailableDates { get; set; } = string.Empty;

	[Display(Name = "")]
	[Info("")]
	public string? InvalidBookDates { get; set; } = string.Empty;

	[Display(Name = "Invalid Book Dates")]
	[Info("")]
	public string? InvalidRooms { get; set; } = string.Empty;

	[Display(Name = "Invalid Nights")]
	[Info("")]
	public string? InvalidNights { get; set; } = string.Empty;

	[Display(Name = "Disabled By Rate Plan")]
	[Info("")]
	public string? DisabledByRatePlan { get; set; } = string.Empty;

	[Display(Name = "Enter Email")]
	[Info("")]
	public string? EnterEmail { get; set; } = string.Empty;

	[Display(Name = "One-Time Invalid")]
	[Info("")]
	public string? OneTimeInvalid { get; set; } = string.Empty;

	[Display(Name = "One-Time Expired")]
	[Info("")]
	public string? OneTimeExpired { get; set; } = string.Empty;

	[Display(Name = "One-TimecUsed")]
	[Info("")]
	public string? OneTimeUsed { get; set; } = string.Empty;

	[Display(Name = "Invalid Booking")]
	[Info("")]
	public string? InvalidBooking { get; set; } = "Invalid Booking";

	[Display(Name = "Invalid Booking Status")]
	[Info("")]
	public string? InvalidBookingStatus { get; set; } = string.Empty;


	// Todo - Discount Error - RoomLimit - Not used
	[Display(Name = "Room Limit")]
	[Info("")]
	public string? RoomLimit { get; set; } = "The discount is not available for these dates.";

	[Display(Name = "Invalid Depart Date")]
	[Info("")]
	public string? InvalidDepartDate { get; set; } = "The offer is not available until after departure.";
}
