using System.Text.Json.Serialization;

namespace Msh.HotelCache.Models.Hotels;

public class HotelDateItem
{
    // The dates when the booking is being made.
    public DateTime BookFromTime { get; set; } = DateTime.MaxValue;
    public DateTime BookToTime { get; set; } = DateTime.MaxValue;

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    [JsonIgnore]
    public bool HasBookDates => BookFromTime != DateTime.MaxValue && BookToTime != DateTime.MaxValue;
    public string Prompt { get; set; } = string.Empty;

    //returns true if disabled
    //|------|              |----| OK
    //          
    //        |- Disabled -|
    //      |----| |---|  |----| disable
    //      |------------------| disable
    public bool IsDisabled(DateTime arrive, DateTime depart) => !(depart <= FromDate || arrive > ToDate);

    public bool IsDisabled(DateTime date) => date >= FromDate && date <= ToDate;

    public bool CannotBook(DateTime now)
    {
        var text = $"{now:yyyy-MM-dd HH:mm:ss} {BookFromTime:yyyy-MM-dd HH:mm:ss}";

        var x = DateTime.Compare(now, BookFromTime) > 0 && DateTime.Compare(now, BookToTime) < 0;
        return x;
    }
}