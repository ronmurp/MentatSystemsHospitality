using System.ComponentModel.DataAnnotations;

namespace Msh.HotelCache.Models.Discounts;

public class DiscountOneTime
{
    [Display(Name="Mode")]
    public OneTimeMode Mode { get; set; } = OneTimeMode.None;

    [Display(Name = "Hash Method")]
	public OneTimeHashVersion HashMethod { get; set; } = OneTimeHashVersion.None;

	[Display(Name = "Requires Reservation Id")]
	public bool RequiresReservationId { get; set; }

	[Display(Name = "Requires Resv Id")]
	public bool RequiresResvId { get; set; }

	[Display(Name = "Requires Profile Id")]
	public bool RequiresProfileId { get; set; }

	[Display(Name = "Requires Email")]
	public bool RequiresEmail { get; set; }

	[Display(Name = "Requires Login")]
	public bool RequiresLogin { get; set; }

	[Display(Name = "Requires Past Depart")]
	public bool RequiresPastDepart { get; set; }

	[Display(Name = "Booking Status")]
	public BookingStatus BookingStatus { get; set; } = BookingStatus.Any;

	[Display(Name = "Allow Rollover")]
	public bool AllowRollover { get; set; }

	[Display(Name = "Update Booker")]
	public bool UpdateBooker { get; set; }

	[Display(Name = "Expire Count")]
	public int ExpireCount { get; set; } = 0;

	[Display(Name = "Expire Count Mode")]
	public ExpireCountMode ExpireCountMode { get; set; } = ExpireCountMode.None;
}