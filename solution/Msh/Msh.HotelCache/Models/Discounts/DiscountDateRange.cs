using Msh.Common.Constants;
using Msh.Common.Models.Dates;

namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// A date range that sets specific discounts on certain days in a range
/// </summary>
/// <remarks>
/// The <see cref="OfferDate"/> determines the validity of the discount code.
/// If any discount date ranges are supplied, they determine the discount on those dates.
/// Examples:
/// 
/// 1 - Single DiscountDateRange: StayFrom/To match OfferDate,
///     but DayRates set a different rate per day
/// 
/// 2 - Multiple DiscountDateRange: Whole OfferDate period is broken down.
///     All days in a DiscountDateRange might have the same discount.
///
/// 3 - Multiple DiscountDateRange: Variation over the OfferDate, and by day of week
/// </remarks>
public class DiscountDateRange : StayDates
{
    public DiscountDateRange()
    {
        DayRates = new Dictionary<string, int>
        {
            { WeekDays.Mon, 0 },
            { WeekDays.Tue, 0 },
            { WeekDays.Wed, 0 },
            { WeekDays.Thu, 0 },
            { WeekDays.Fri, 0 },
            { WeekDays.Sat, 0 },
            { WeekDays.Sun, 0 }
        };
    }
    public Dictionary<string, int> DayRates { get; private set; }
}