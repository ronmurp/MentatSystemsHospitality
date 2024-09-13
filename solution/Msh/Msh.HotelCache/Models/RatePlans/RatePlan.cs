using System.ComponentModel.DataAnnotations;
using Msh.Common.Attributes;
using Msh.Common.Models;

namespace Msh.HotelCache.Models.RatePlans;

public class RoomRatePlan
{
    [Required][Display(Name = "Rate Plan Code"), Info("rate-plan-code")]
    public string RatePlanCode { get; set; } = string.Empty;

	/// <summary>
	/// Text to add to confirmation page
	/// </summary>
	[Display(Name = "Confirmation Text"), Info("confirmation-text")]
	public string? ConfirmationText { get; set; } = string.Empty;

	/// <summary>
	/// When an offer is not available in the search results
	/// </summary>
	[Display(Name = "Default Description"), Info("default-desc")] 
	public string? DefaultDescription { get; set; } = string.Empty;

	[Display(Name = "Group"), Info("group")]
	public string? Group { get; set; } = string.Empty;

	[Display(Name = "Title"), Info("title")]
	public string? Title { get; set; } = string.Empty;


	[Display(Name = "Sub-Title"), Info("sub-title")]
	public string? SubTitle { get; set; } = string.Empty;


	[Display(Name = "Description"), Info("description")]
	public string? Description { get; set; } = string.Empty;

	[Display(Name = "Short Description"), Info("short-description")]
	public string? ShortDescription { get; set; } = string.Empty;

	/// <summary>
	/// When an offer (via a code) is not available in the results screen
	/// See ResultsHtmlBuilder.BuildSpecialOffers
	/// </summary>
	[Display(Name = "Unavailable Offer"), Info("unavailable-offer")] 
	public string? UnavailableOffer { get; set; } = string.Empty;

	[Display(Name = "Market Segment"), Info("market-segment")]
	public string? MarketSegment { get; set; } = string.Empty;


	[Display(Name = "Source Code"), Info("source-code")]
	public string? SourceCode { get; set; } = string.Empty;


	[Display(Name = "Is Base Rate"), Info("is-base-rate")] 
	public bool IsBaseRate { get; set; }

	[Display(Name = "Is Offer"), Info("is-offer")]
	public bool IsOffer { get; set; }

	[Display(Name = "Is Promotion"), Info("is-promotion")]
	public bool IsPromo { get; set; }

	/// <summary>
	/// Originally isDBB was Is Dinner + B&B, as opposed to just Is BB
	/// Now also represents isRBB (Resort + B&B)
	/// </summary>
	[Display(Name = "Is DBB"), Info("is-dbb")] 
	public bool IsDbb { get; set; }


	[Display(Name = "DBB Text"), Info("dbb-text")]
	public string? DbbText { get; set; } = string.Empty;

	
	[Display(Name = "Full Pre-Pay"), Info("full-pre-pay")]
	public bool FullPrePay { get; set; }

	[Display(Name = "Deposit Days"), Info("deposit-days")]
	public int DepositDays { get; set; }

	[Display(Name = "Deposit Amount"), Info("deposit-amount")]
	[DataType(DataType.Currency)]
	public int DepositAmount { get; set; }

	/// <summary>
	/// This will override the "balance on departure" message 
	/// Applies for deposits only
	/// </summary>
	/// <remarks>
	/// Pre-payment: Payment page
	/// Post-payment: Confirmation page, confirmation email
	/// </remarks>
	[Display(Name = "Deposit Balance Message"), Info("deposit-balance-message")] 
	public string? DepositBalanceMessage { get; set; } = string.Empty;

	[Display(Name = "Stay From"), Info("stay-from")]
	[DataType(DataType.Date)]
	public DateOnly StayFrom { get; set; }

	[Display(Name = "Stay To"), Info("stay-to")]
	[DataType(DataType.Date)]
	public DateOnly StayTo { get; set; }

	[Display(Name = "Has Book Dates"), Info("has-book-dates")]
	public bool HasBookDates { get; set; }

	[Display(Name = "Book From"), Info("book-from")]
	[DataType(DataType.Date)]
	public DateOnly BookFrom { get; set; }

	[Display(Name = "Book To"), Info("book-to")]
	[DataType(DataType.Date)]
	public DateOnly BookTo { get; set; }

	[Display(Name = "Group Sort Order"), Info("group-sort-order")]
	public int GroupSortOrder { get; set; } = 0;

	[Display(Name = "Min Nights"), Info("min-nights")]
	public int MinNights { get; set; }

	[Display(Name = "Max Nights"), Info("max-nights")]
	public int MaxNights { get; set; }


	/// <summary>
	/// If true, disabled this rate on all discounts - though it will still appear in a list of offers, even for discounts.
	/// </summary>
	[Display(Name = "Disable Discount"), Info("disable-discount")] 
	public bool DisableDiscount { get; set; }

	/// <summary>
	/// If present, this rate plan can be used ONLY when one of the discount codes is active
	/// </summary>
	[Display(Name = "With Discounts"), Info("with-discounts")] 
	public List<string> WithDiscounts { get; set; } = [];

	[Display(Name = "Booking Comments"), Info("booking-comments")]
	public List<CommonText> BookingComments { get; set; } = [];

	[Display(Name = "FIT Enabled"), Info("fit-enabled")]
	public bool FitEnabled { get; set; }

	/// <summary>
	/// Names the base RatePlanCode that the FitAgent is going to use
	/// </summary>
	[Display(Name = "Base Rate"), Info("base-rate")] 
	public string? BaseRate { get; set; } = string.Empty;

	/// <summary>
	/// ELH internal notes
	/// </summary>
	[Display(Name = "Notes"), Info("notes")]
	[DataType(DataType.MultilineText)]
	public string? Notes { get; set; } = string.Empty;

}