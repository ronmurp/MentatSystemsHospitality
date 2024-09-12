using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Msh.Common.Attributes;

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
	[Display(Name = "Room Type Code"), Info("room-type-code")]
	public string Code { get; set; } = string.Empty;


	/// <summary>
	/// A group code, used when rooms are grouped
	/// </summary>

	[Display(Name = "Group Code"), Info("group-code")]
	public string? GroupCode { get; set; } = string.Empty;

	/// <summary>
	/// Number of people in the room. May be adults, adults+children, adults+children+infants
	/// Depends on Opera etc.
	/// </summary>

	[Display(Name = "Occupation"), Info("occupation")]
	public int Occupancy { get; set; } = 2;

	/// <summary>
	/// A short text name for the room type
	/// </summary>

	[Display(Name = "Name"), Info("name")]
	public string? Name { get; set; } = string.Empty;

	/// <summary>
	/// A short single line text description
	/// </summary>

	[Display(Name = "Description"), Info("description")]
	public string? Description { get; set; } = string.Empty;

	/// <summary>
	/// HTML description for the room
	/// </summary>
	[Display(Name = "HTML Description"), Info("html-description"), Category("Html")]
	public string? HtmlDescription { get; set; } = string.Empty;

	/// <summary>
	/// Image file name. Image files are expected to be found in a location determined elsewhere
	/// </summary>
	[Display(Name = "Image File"), Info("image-file")]
	public string? ImageFile { get; set; } = string.Empty;

	/// <summary>
	/// Number of rooms of this type
	/// </summary>
	[Display(Name = "Room Count"), Info("room-count")]
	public int RoomCount { get; set; }

	/// <summary>
	/// Category of room. Similar to group code, but this is a descriptive name, not a code
	/// </summary>

	[Display(Name = "Room Category"), Info("room-category")]
	public string? RoomCategory { get; set; } = string.Empty;


	/// <summary>
	/// Is the room type blocked for use
	/// </summary>

	[Display(Name = "Room Blocked"), Info("room-blocked")]
	public bool RoomBlocked { get; set; }


	/// <summary>
	/// Is there a Twin/Double/Zipped option
	/// </summary>

	[Display(Name = "Twin Option"), Info("twin-option")]
	public bool TwinOption { get; set; }


	/// <summary>
	/// Codes used as required: DBL/TWN/ZP
	/// </summary>

	[Display(Name = "Option Type Code"), Info("option-type-code")]
	public string? OptionTypeCode { get; set; } = string.Empty;

	/// <summary>
	/// When options are restricted ; 'Double only'
	/// </summary>

	[Display(Name = "Otion Warning"), Info("option-warning")]
	public string? OptionTypeWarning { get; set; } = string.Empty;


	/// <summary>
	/// Html that contains a list of image elements representing the gallery.
	/// Being HTML it can be changed to suit different gallery scripts
	/// </summary>
	[Display(Name = "Image Gallery"), Info("image-gallery")]
	public string? ImageGallery { get; set; } = string.Empty;



	[Display(Name = "GroupName"), Info("group-name")]
	public string? GroupName { get; set; } = string.Empty;


	[Display(Name = "Group Description"), Info("group-description")]
	public string? GroupDescription { get; set; } = string.Empty;

	/// <summary>
	/// HTML description for the room group, where room types
	/// </summary>
	[Display(Name = "Group HTML Description"), Info("group-html-description")]
	public string? GroupLongDescription { get; set; } = string.Empty;



    [Display(Name="Group Image"), Info("group-image")]
	public string? GroupImageFile { get; set; } = string.Empty;


    [Display(Name = "Group Image Gallery"), Info("group-image-gallery"), Category("Html")]
	public string? GroupImageGallery { get; set; } = string.Empty;


	[Display(Name = "Group Twin Option"), Info("group-twin-description")]
	public bool GroupTwinOption { get; set; }


	[Display(Name = "No Dogs"), Info("no-dogs")]
	public bool NoDogs { get; set; }



	[Display(Name = "No Dogs Text"), Info("no-dogs-text")]
	public string? NoDogsText { get; set; } = string.Empty;

}