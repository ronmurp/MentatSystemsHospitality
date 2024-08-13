namespace Msh.HotelCache.Models;

public class RoomType : BaseRoomType
{
    public string GroupShortDescription { get; set; } = string.Empty;
    public string GroupLongDescription { get; set; } = string.Empty;
    public string GroupImageFile { get; set; } = string.Empty;
    public string GroupImageFileGallery { get; set; } = string.Empty;
    public bool NoDogs { get; set; }
    public string NoDogsText { get; set; } = string.Empty;
}