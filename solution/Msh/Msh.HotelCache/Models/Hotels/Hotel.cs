namespace Msh.HotelCache.Models.Hotels;

/// <summary>
/// Data representing a single hotel
/// </summary>
public class Hotel
{
    /// <summary>
    /// Intended to be used for OWS, but isn't. See OwsConfig.ChainCode.
    /// </summary>
    public string ChainCode { get; set; } = string.Empty;

    /// <summary>
    /// The code that identifies the hotel.
    /// </summary>
    /// <remarks>
    /// Note that this might not be the same as used elsewhere (outside WBS/OWS/Opera).
    /// Example: LWH is often seen as LWB.
    /// </remarks>
    public string HotelCode { get; set; } = string.Empty;

    /// <summary>
    /// The name of the hotel
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// If XML, this is usually pulled from an html file.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Not used
    /// </summary>
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// Displayed on the confirmation page, and in emails.
    /// </summary>
    public string ConfirmCopy { get; set; } = string.Empty;

    /// <summary>
    /// An image of the hotel. Used in RoomBookingList, but not used from there
    /// </summary>
    public string ImageFile { get; set; } = string.Empty;

    /// <summary>
    /// Not used
    /// </summary>
    public string SearchImageFile { get; set; } = string.Empty;

    /// <summary>
    /// Numbers for guests to call for help
    /// </summary>
    public string CrsNumber { get; set; } = string.Empty;
    public string CrsNumberInt { get; set; } = string.Empty;

    public List<HotelDateItem> HotelDateList { get; set; } = new List<HotelDateItem>();

    public string DisabledText { get; set; } = string.Empty;

    /// <summary>
    /// Introduced for when a hotel does not allow dogs in any rooms
    /// </summary>
    public bool NoDogs { get; set; }

    /// <summary>
    /// Introduced for when a hotel does not allow dogs in any rooms
    /// </summary>
    public string NoDogsText { get; set; } = string.Empty;
}