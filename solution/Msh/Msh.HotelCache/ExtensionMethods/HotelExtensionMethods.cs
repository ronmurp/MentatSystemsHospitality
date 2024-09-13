using Msh.Common.ExtensionMethods;
using Msh.Common.Models.Dates;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.ExtensionMethods;

public static class HotelExtensionMethods
{
    public static bool HasDates(this Hotel hotel) => 
        hotel.StayDates.Count > 0;

    public static bool IsDisabled(this Hotel hotel, DateOnly arrive, DateOnly depart) => 
        hotel.StayDates.Any(hdi => hdi.IsDisabled(arrive, depart)) 
        || !string.IsNullOrEmpty(hotel.DisabledText);

    public static ItemDate? InvalidDate(this Hotel hotel, DateOnly arrive, DateOnly depart) => 
        hotel.StayDates.FirstOrDefault(d => d.IsDisabled(arrive, depart));

    public static bool CannotBook(this Hotel hotel, DateOnly now) => 
        hotel.StayDates.Exists(d => d.CannotBook(now));

    public static ItemDate? CannotBookDate(this Hotel hotel, DateOnly now) => 
        hotel.StayDates.FirstOrDefault(d => d.CannotBook(now));

}