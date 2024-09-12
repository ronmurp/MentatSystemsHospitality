namespace Msh.HotelCache.Models.RoomTypes;

public class HotelRoomTypesFilters
{
    private const int DefaultMaxAdults = 3;
    private const int DefaultMaxChildren = 3;
    private const int DefaultMaxInfants = 3;

    public string HotelCode { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public int MaxAdults { get; set; } = DefaultMaxAdults;
    public int MaxChildren { get; set; } = DefaultMaxChildren;
    public int MaxInfants { get; set; } = DefaultMaxInfants;

    public List<RoomTypeFilters> RoomTypesFiltersList { get; set; } = [];

    //public RoomTypeFilters? GetRoomTypeFilters(string roomTypeCode, string key) =>
    //    RoomTypesFiltersList.FirstOrDefault(rt =>
    //        rt.RoomTypeCode.Equals(roomTypeCode) && rt.RoomTypeFiltersList.Exists(f => f.Key.Equals(key)));

    //public bool IsBlocked(string roomTypeCode, string key) =>
    //    GetRoomTypeFilters(roomTypeCode, key)?.IsBlocked(key) ?? false;
}