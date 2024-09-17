using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Msh.Common.Attributes;
using Msh.Common.Models.Dates;

namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// Discount codes apply on top of any Opera/OWS room pricing
/// </summary>
public class DiscountCode
{
    [Display(Name="Discount Code")]
    [Info("discount-code")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Enabled")]
    [Info("enabled")]
    public bool Enabled { get; set; } = false;

	/// <summary>
	/// If true, this code is presented for selection on the payment page
	/// </summary>
	[Display(Name = "Selectable")]
	[Info("selectable")] 
	public bool Selectable { get; set; } = false;

	[Display(Name = "Min. Rooms")]
    [Info("min-rooms")]
	public int MinRooms { get; set; } = 1;

	[Display(Name = "Max. Rooms")]
	[Info("max-rooms")]
	public int MaxRooms { get; set; } = 10;

	[Display(Name = "Min. Nights")]
	[Info("min-nights")]
	public int MinNights { get; set; } = 1;

	[Display(Name = "Max. Nights")]
	[Info("max-rooms")]
	public int MaxNights { get; set; } = 14;

	[Display(Name = "Discount Type")]
	[Info("discount-type")]
	public DiscountTypes DiscountType { get; set; } = DiscountTypes.None;

    public DiscountOneTime OneTime { get; set; } = new DiscountOneTime();

    /// <summary>
    /// A value that's either a representation of a number of rooms or nights, or a percent
    /// </summary>
    [Display(Name="Discount Value")]
    [Info("discount-value")]
	public int Discount { get; set; }

	[Display(Name = "Description")]
	[Info("discription")]
	public string? Description { get; set; } = string.Empty;

	/// <summary>
	/// Additional text details, such as the dates
	/// </summary>
	[Display(Name = "Details")]
	[Info("details")]
	public string? Details { get; set; } = string.Empty;

	/// <summary>
	/// If not empty, then emails are required to validate the application of the discount
	/// </summary>
	[Display(Name = "Email Validation List")]
	[Info("emails")] 
	public string? EmailValidationList { get; set; } = string.Empty;

    /// <summary>
    /// The date ranges for which the discount applies (and potentially booking dates)
    /// </summary>
    public List<ItemDate> OfferDates { get; set; } = [];

    public List<ItemDate> BookDates { get; set; } = [];

    public DiscountErrors DiscountErrors { get; set; } = new DiscountErrors();

    public List<string> DisabledHotelPlans { get; set; } = [];
    public List<string> EnabledHotelPlans { get; set; } = [];

    [Display(Name = "Short Description")]
    [Info("short-description")]
	public string? ShortDescription { get; set; } = string.Empty;

	/// <summary>
	/// Requires the user to copy-paste some code, or even the discount code ... depends on the type
	/// </summary>
	[Display(Name = "Requires Copy-Paste")]
	[Info("copy-paste")] 
	public bool RequiresCopyPaste { get; set; }

	/// <summary>
	/// Default true for backward compatibility with older discounts that are all fullPrepay.
	/// This can be used with rate plan to override that
	/// </summary>
	[Display(Name = "Full Pre-pay")]
	[Info("full-pre-pay")] 
	public bool FullPrepay { get; set; } = true;

	/// <summary>
	/// The minimum amount that can be charged when the discount would otherwise result in a zero or negative charge.
	/// </summary>
	[Display(Name = "Min. Total")]
	[Info("min-total")]
	public decimal MinTotal { get; set; } = 1M; // £1 minimum after discount

	/// <summary>
	/// The message displayed when a minimum total is applied because:
	/// - the calculated total is less than or equal to zero
	/// - the payment limit (MinTotal) is being applied
	/// </summary>
	[Display(Name = "Min. Total Message")]
	[Info("min-total-message")] 
	public string? MinTotalMessage { get; set; } = string.Empty;

	/// <summary>
	/// The message displayed when the intended DiscountOff is greater than the rooms total
	/// and therefore is limited to the rooms total
	/// </summary>
	[Display(Name = "Limited Discount Message")]
	[Info("limited-discount-message")] 
	public string? LimitedDiscountMessage { get; set; } = string.Empty;

    //[JsonIgnore]
    //public bool HasDayRates => DiscountType == DiscountTypes.Percent && DayRates.Count > 0;

    public List<DiscountDateRange> DayRates { get; set; } = [];

	/// <summary>
	/// Overrides the standard prompt, if present
	/// </summary>
	[Display(Name = "Discount Warning")]
	[Info("discount-warning")] 
	public string? DiscountWarning { get; set; } = string.Empty;

	[Description("Notes"), Info("extra-notes")]
	public string? Notes { get; set; } = string.Empty;
}