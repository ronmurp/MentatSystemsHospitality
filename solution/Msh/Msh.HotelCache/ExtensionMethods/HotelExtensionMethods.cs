using Msh.HotelCache.Models;

namespace Msh.HotelCache.ExtensionMethods;

public static class HotelExtensionMethods
{
    public static bool HasDates(this Hotel hotel) => 
        hotel.HotelDateList.Count > 0;

    public static bool IsDisabled(this Hotel hotel, DateTime arrive, DateTime depart) => 
        hotel.HotelDateList.Any(hdi => hdi.IsDisabled(arrive, depart)) 
        || !string.IsNullOrEmpty(hotel.DisabledText);

    public static HotelDateItem? InvalidDate(this Hotel hotel, DateTime arrive, DateTime depart) => 
        hotel.HotelDateList.FirstOrDefault(d => d.IsDisabled(arrive, depart));

    public static bool CannotBook(this Hotel hotel, DateTime now) => 
        hotel.HotelDateList.Exists(d => d.CannotBook(now));

    public static HotelDateItem? CannotBookDate(this Hotel hotel, DateTime now) => 
        hotel.HotelDateList.FirstOrDefault(d => d.CannotBook(now));

}