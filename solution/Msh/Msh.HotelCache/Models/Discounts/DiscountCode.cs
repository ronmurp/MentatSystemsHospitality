using System.Text.Json.Serialization;
using Msh.Common.Models.Dates;

namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// Discount codes apply on top of any Opera/OWS room pricing
/// </summary>
public class DiscountCode
{
    public string Code { get; set; } = string.Empty;

    public int MinRooms { get; set; } = 1;
    public int MaxRooms { get; set; } = 10;

    public int MinNights { get; set; } = 1;
    public int MaxNights { get; set; } = 14;

    public DiscountTypes DiscountType { get; set; } = DiscountTypes.None;

    public DiscountOneTime OneTime { get; set; } = new DiscountOneTime();

    /// <summary>
    /// A value that's either a representation of a number of rooms or nights, or a percent
    /// </summary>
    public int Discount { get; set; }

    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Additional text details, such as the dates
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// If not empty, then emails are required to validate the application of the discount
    /// </summary>
    public string EmailValidationList { get; set; } = string.Empty;

    /// <summary>
    /// The date ranges for which the discount applies (and potentially booking dates)
    /// </summary>
    public List<OfferDate> OfferDates { get; set; } = [];

    /// <summary>
    /// The list of hotels to which the discount applies
    /// </summary>
    public List<string> Hotels { get; set; } = [];

    public bool Enabled { get; set; } = false;

    /// <summary>
    /// If true, this code is presented for selection on the payment page
    /// </summary>
    public bool Selectable { get; set; } = false;

    public DiscountErrors DiscountErrors { get; set; } = new DiscountErrors();

    public List<DiscountHotelRatePlans> DisabledHotelPlans { get; set; } = [];
    public List<DiscountHotelRatePlans> EnabledHotelPlans { get; set; } = [];
    public string ShortDescription { get; set; } = string.Empty;

    /// <summary>
    /// Requires the user to copy-paste some code, or even the discount code ... depends on the type
    /// </summary>
    public bool RequiresCopyPaste { get; set; }

    /// <summary>
    /// Default true for backward compatibility with older discounts that are all fullPrepay.
    /// This can be used with rate plan to override that
    /// </summary>
    public bool FullPrepay { get; set; } = true;

    /// <summary>
    /// The minimum amount that can be charged when the discount would otherwise result in a zero or negative charge.
    /// </summary>
    public decimal MinTotal { get; set; } = 1M; // £1 minimum after discount

    /// <summary>
    /// The message displayed when a minimum total is applied because:
    /// - the calculated total is less than or equal to zero
    /// - the payment limit (MinTotal) is being applied
    /// </summary>
    public string MinTotalMessage { get; set; } = string.Empty;

    /// <summary>
    /// The message displayed when the intended DiscountOff is greater than the rooms total
    /// and therefore is limited to the rooms total
    /// </summary>
    public string LimitedDiscountMessage { get; set; } = string.Empty;

    [JsonIgnore]
    public bool HasDayRates => DiscountType == DiscountTypes.Percent && DayRates.Count > 0;

    public List<DiscountDateRange> DayRates { get; set; } = [];

    /// <summary>
    /// Overrides the standard prompt, if present
    /// </summary>
    public string DiscountWarning { get; set; } = string.Empty;
}