namespace Msh.HotelCache.Models.RoomTypes;

public class RoomTypeFilter
{

    public int Adults { get; set; }
    public int Children { get; set; }
    public int Infants { get; set; }

    public bool Block { get; set; }

    //public bool Equals(RoomTypeFilter obj)
    //{
    //    return obj.Adults == Adults && obj.Children == Children && obj.Infants == Infants;
    //}

    //[JsonIgnore]
    //public string Key => $"{Adults}-{Children}-{Infants}";
}