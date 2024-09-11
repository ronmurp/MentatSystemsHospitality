using Msh.Common.ExtensionMethods;
using Msh.Common.Models.Dates;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.ExtensionMethods;

public static class HotelExtensionMethods
{
    public static bool HasDates(this Hotel hotel) => 
        hotel.HotelDateList.Count > 0;

    public static bool IsDisabled(this Hotel hotel, DateOnly arrive, DateOnly depart) => 
        hotel.HotelDateList.Any(hdi => hdi.IsDisabled(arrive, depart)) 
        || !string.IsNullOrEmpty(hotel.DisabledText);

    public static HotelDateItem? InvalidDate(this Hotel hotel, DateOnly arrive, DateOnly depart) => 
        hotel.HotelDateList.FirstOrDefault(d => d.IsDisabled(arrive, depart));

    public static bool CannotBook(this Hotel hotel, DateOnly now) => 
        hotel.HotelDateList.Exists(d => d.CannotBook(now));

    public static HotelDateItem? CannotBookDate(this Hotel hotel, DateOnly now) => 
        hotel.HotelDateList.FirstOrDefault(d => d.CannotBook(now));

}