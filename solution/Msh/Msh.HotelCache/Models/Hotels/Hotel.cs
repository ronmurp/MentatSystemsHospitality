using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Msh.Common.Attributes;

namespace Msh.HotelCache.Models.Hotels;

/// <summary>
/// Data representing a single hotel
/// </summary>
public class Hotel
{
    /// <summary>
    /// Intended to be used for OWS, but isn't. See OwsConfig.ChainCode.
    /// </summary>
    [Required]
    [Description("Chain Code"), Info("chain-code")] 
    public string ChainCode { get; set; } = string.Empty;

  
    /// <summary>
    /// The code that identifies the hotel.
    /// </summary>
    /// <remarks>
    /// Note that this might not be the same as used elsewhere (outside WBS/OWS/Opera).
    /// Example: LWH is often seen as LWB.
    /// </remarks>
    [Required]
	[Description("Hotel Code"), Info("hotel-code")] 
    public string HotelCode { get; set; } = string.Empty;

    /// <summary>
    /// The name of the hotel
    /// </summary>
    [Required][Description("Name"), Info("name")] 
    public string Name { get; set; } = string.Empty;

	/// <summary>
	/// If XML, this is usually pulled from a html file.
	/// </summary>
	[Required][Description("Description"), Info("description")] 
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Not used
	/// </summary>
	[Required]
	[Description("Subtitle"), Info("subtitle")]
	public string Subtitle { get; set; } = string.Empty;

	/// <summary>
	/// Displayed on the confirmation page, and in emails.
	/// </summary>
	[Required]
	[Description("Confirm Copy"), Info("confirm-copy")]
	public string ConfirmCopy { get; set; } = string.Empty;

	/// <summary>
	/// An image of the hotel. Used in RoomBookingList, but not used from there
	/// </summary>
	[Required]
	[Description("Image File"), Info("image-file")]
	public string ImageFile { get; set; } = string.Empty;

	/// <summary>
	/// Not used
	/// </summary>
	[Required]
	[Description("Search Image File"), Info("search-image-file")]
	public string SearchImageFile { get; set; } = string.Empty;

	/// <summary>
	/// Numbers for guests to call for help
	/// </summary>
	[Required]
	[Description("CRS Number"), Info("crs-number")] 
	public string CrsNumber { get; set; } = string.Empty;
	[Required]
	[Description("CRS Number Int"), Info("crs-number-int")] 
	public string CrsNumberInt { get; set; } = string.Empty;

    public List<HotelDateItem> HotelDateList { get; set; } = new List<HotelDateItem>();

	[Required][Description("Disabled Text"), Info("disabled-text")] 
	public string DisabledText { get; set; } = string.Empty;

	/// <summary>
	/// Introduced for when a hotel does not allow dogs in any rooms
	/// </summary>
	[Description("No Dogs"), Info("no-dogs")] 
	public bool NoDogs { get; set; }

	/// <summary>
	/// Introduced for when a hotel does not allow dogs in any rooms
	/// </summary>
	[Description("No Dogs Text"), Info("no-dogs-text")]
	public string NoDogsText { get; set; } = string.Empty;

	/// <summary>
	/// Notes that are only available to admin
	/// </summary>
	[Description("Admin Notes"), Info("admin-notes"), Category("TextArea")] 
	public string AdminNotes { get; set; } = string.Empty;
}