namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// The expire count mode, for discounts hat can expire, in DiscountCodes.xml
/// </summary>
public enum ExpireCountMode
{
    None, Days, Weeks, MonthsToday, MonthsCalendar
}