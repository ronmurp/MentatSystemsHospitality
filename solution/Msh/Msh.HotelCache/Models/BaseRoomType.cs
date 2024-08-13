namespace Msh.HotelCache.Models;

public class BaseRoomType
{
    public const string TwinOptionTwin = "Twin";
    public const string TwinOptionDouble = "Double";

    public string Code { get; set; } = string.Empty;
    public string GroupCode { get; set; } = string.Empty;
    public int Occupancy { get; set; }
    public string Description { get; set; } = string.Empty;
    public string GroupDescription { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string ImageFile { get; set; } = string.Empty;
    public int RoomCount { get; set; }

    public string RoomCategory { get; set; } = string.Empty;
    public bool RoomBlocked { get; set; }
    public bool TwinOption { get; set; }
    public bool GroupTwinOption { get; set; }
    public string OptionTypeCode { get; set; } = string.Empty;
    public string OptionTypeWarning { get; set; } = string.Empty;
    public string ImageFileGallery { get; set; } = string.Empty;
}