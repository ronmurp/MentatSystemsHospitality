using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Msh.Common.Attributes;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Models.RoomTypes;

/// <summary>
/// A room type to be offered for accommodation
/// </summary>
/// <remarks>
/// Depending on how the Opera room types are to be offered:
/// - As in Opera - each room type is unique and offered as that type
/// - In groups - Our own grouping of room types, to reduce variety offered
/// Rooms used in groups should be similar for features and occupancy
///
/// In configuration, a list of room types for each hotel is saved in its own hotel configuration
/// $"RoomTypes-{HotelCode}"
/// </remarks>
public class RoomType
{

	public const string TwinOptionTwin = "Twin";
	public const string TwinOptionDouble = "Double";

	/// <summary>
	/// The room type code. This must correspond to the accommodation API's room types
	/// </summary>
	[Required]
	[Description("Room Type Code"), Info("room-type-code")]
	public string Code { get; set; } = string.Empty;


	/// <summary>
	/// A group code, used when rooms are grouped
	/// </summary>

	[Description("Group Code"), Info("group-code")]
	public string? GroupCode { get; set; } = string.Empty;

	/// <summary>
	/// Number of people in the room. May be adults, adults+children, adults+children+infants
	/// Depends on Opera etc.
	/// </summary>

	[Description("Occupation"), Info("occupation")]
	public int Occupancy { get; set; } = 2;

	/// <summary>
	/// A short text name for the room type
	/// </summary>

	[Description("Name"), Info("name")]
	public string? Name { get; set; } = string.Empty;

	/// <summary>
	/// A short single line text description
	/// </summary>

	[Description("Description"), Info("description")]
	public string? Description { get; set; } = string.Empty;

	/// <summary>
	/// HTML description for the room
	/// </summary>
	[Description("HTML Description"), Info("html-description"), Category("Html")]
	public string? HtmlDescription { get; set; } = string.Empty;

	/// <summary>
	/// Image file name. Image files are expected to be found in a location determined elsewhere
	/// </summary>
	[Description("Image File"), Info("image-file")]
	public string? ImageFile { get; set; } = string.Empty;

	/// <summary>
	/// Number of rooms of this type
	/// </summary>
	[Description("Room Count"), Info("room-count")]
	public int RoomCount { get; set; }

	/// <summary>
	/// Category of room. Similar to group code, but this is a descriptive name, not a code
	/// </summary>

	[Description("Room Category"), Info("room-category")]
	public string? RoomCategory { get; set; } = string.Empty;


	/// <summary>
	/// Is the room type blocked for use
	/// </summary>

	[Description("Room Blocked"), Info("room-blocked")]
	public bool RoomBlocked { get; set; }


	/// <summary>
	/// Is there a Twin/Double/Zipped option
	/// </summary>

	[Description("Twin Option"), Info("twin-option")]
	public bool TwinOption { get; set; }


	/// <summary>
	/// Codes used as required: DBL/TWN/ZP
	/// </summary>

	[Description("Option Type Code"), Info("option-type-code")]
	public string? OptionTypeCode { get; set; } = string.Empty;

	/// <summary>
	/// When options are restricted ; 'Double only'
	/// </summary>

	[Description("Otion Warning"), Info("option-warning")]
	public string? OptionTypeWarning { get; set; } = string.Empty;


	/// <summary>
	/// Html that contains a list of image elements representing the gallery.
	/// Being HTML it can be changed to suit different gallery scripts
	/// </summary>
	[Description("Image Gallery"), Info("image-gallery")]
	public string? ImageGallery { get; set; } = string.Empty;



	[Description("GroupName"), Info("group-name")]
	public string? GroupName { get; set; } = string.Empty;


	[Description("Group Description"), Info("group-description")]
	public string? GroupDescription { get; set; } = string.Empty;

	/// <summary>
	/// HTML description for the room group, where room types
	/// </summary>
	[Description("Group HTML Description"), Info("group-html-description")]
	public string? GroupLongDescription { get; set; } = string.Empty;



    [Description("Group Image"), Info("group-image")]
	public string? GroupImageFile { get; set; } = string.Empty;


    [Description("Group Image Gallery"), Info("group-image-gallery"), Category("Html")]
	public string? GroupImageGallery { get; set; } = string.Empty;


	[Description("Group Twin Option"), Info("group-twin-description")]
	public bool GroupTwinOption { get; set; }


	[Description("No Dogs"), Info("no-dogs")]
	public bool NoDogs { get; set; }



	[Description("No Dogs Text"), Info("no-dogs-text")]
	public string? NoDogsText { get; set; } = string.Empty;

	public List<HotelDateItem> Dates { get; set; } = [];
}