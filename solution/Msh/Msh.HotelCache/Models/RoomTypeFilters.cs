namespace Msh.HotelCache.Models;

public class RoomTypeFilters
{
    public string RoomTypeCode { get; set; } = string.Empty;

    public int Order { get; set; }

    public int LowLimit { get; set; }


    public List<RoomTypeFilter> RoomTypeFiltersList { get; set; } = [];

    public RoomTypeFilter? FindByKey(string key) => 
        RoomTypeFiltersList.FirstOrDefault(f => f.Key == key);
    public bool Exists(string key) => 
        RoomTypeFiltersList.Any(f => f.Key == key);
    public bool IsBlocked(string key) => 
        RoomTypeFiltersList.Find(v => v.Key == key && v.Block) != null;
    public bool HasKeyNotBlocked(string key) => 
        RoomTypeFiltersList.Find(v => v.Key == key && !v.Block) != null;
}