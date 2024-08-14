using System.Text.Json.Serialization;
using Msh.Common.Models;
using Msh.Common.Models.Dates;

namespace Msh.HotelCache.Models.RatePlans;

public class RoomRatePlan : BaseTexts
{
    /// <summary>
    /// a working value - not saved
    /// </summary>
    public string HotelCode { get; set; } = string.Empty;

    public string RatePlanCode { get; set; } = string.Empty;

    /// <summary>
    /// Text to add to confirmation page
    /// </summary>
    public string ConfirmationText { get; set; } = string.Empty; 

    /// <summary>
    /// When an offer is not available in the search results
    /// </summary>
    public string DefaultDescription { get; set; } = string.Empty;

    /// <summary>
    /// When an offer (via a code) is not available in the results screen
    /// See ResultsHtmlBuilder.BuildSpecialOffers
    /// </summary>
    public string UnavailableOffer { get; set; } = string.Empty;

    public bool IsBaseRate { get; set; }

    public string MarketSegment { get; set; } = string.Empty;

    public bool IsOffer { get; set; }

    public bool IsPromo { get; set; }

    /// <summary>
    /// Originally isDBB was Is Dinner + B&B, as opposed to just Is BB
    /// Now also represents isRBB (Resort + B&B)
    /// </summary>
    public bool IsDbb { get; set; }

    public string DbbText { get; set; } = string.Empty;

    public string Sourcecode { get; set; } = string.Empty;
    public bool FullPrePay { get; set; }
    public int DepositDays { get; set; }
    public int DepositAmount { get; set; }

    /// <summary>
    /// This will override the "balance on departure" message 
    /// Applies for deposits only
    /// </summary>
    /// <remarks>
    /// Pre-payment: Payment page
    /// Post-payment: Confirmation page, confirmation email
    /// </remarks>
    public string DepositBalanceMessage { get; set; } = string.Empty;


    [JsonIgnore]
    public DateTime StayFrom => this.OfferDates.StayFrom;

    [JsonIgnore]
    public DateTime StayTo => this.OfferDates.StayTo;

    public OfferDate OfferDates { get; set; } = new OfferDate();

    public int GroupSortOrder { get; set; } = 0;


    public int MinNights { get; set; }

    public int MaxNights { get; set; }


    /// <summary>
    /// If true, disabled this rate on all discounts - though it will still appear in a list of offers, even for discounts.
    /// </summary>
    public bool DisableDiscount { get; set; }

    /// <summary>
    /// If present, this rate plan can be used ONLY when one of the discount codes is active
    /// </summary>
    public List<string> WithDiscounts { get; set; } = [];

    public List<CommonText> BookingComments { get; set; } = [];

    public bool FitEnabled { get; set; }

    /// <summary>
    /// Names the base RatePlanCode that the FitAgent is going to use
    /// </summary>
    public string BaseRate { get; set; } = string.Empty;

    /// <summary>
    /// ELH internal notes
    /// </summary>
    public string Notes { get; set; } = string.Empty;

}