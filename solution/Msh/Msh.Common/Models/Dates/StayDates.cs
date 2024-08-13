using System.Text.Json.Serialization;
using Msh.Common.Constants;
using Msh.Common.ExtensionMethods;

namespace Msh.Common.Models.Dates;

public class StayDates
{
    // Arrives are allowed on this date
    public DateTime StayFrom { get; set; } = DateTime.MinValue;
    public DateTime EarliestArrive => StayFrom;

    // Departs are allowed on this date
    public DateTime StayTo { get; set; } = DateTime.MaxValue;

    [JsonIgnore]
    public DateTime LatestDepart => StayTo;

    /// <summary>
    /// The arrive and depart are contained within the valid range
    /// </summary>
    /// <param name="arrive"></param>
    /// <param name="depart"></param>
    /// <returns></returns>
    public bool Contains(DateTime arrive, DateTime depart) => arrive >= EarliestArrive && depart <= LatestDepart;
    public bool TooEarlier(DateTime arrive) => arrive < EarliestArrive;
    public bool TooLate(DateTime depart) => depart > LatestDepart;

    [JsonIgnore]
    public string StayFromString => StayFrom.ToString(DateConst.DateFormat);

    [JsonIgnore]
    public string StayToString => StayTo.ToString(DateConst.DateFormat);

    [JsonIgnore]
    public int StayRange => (int)(StayTo.Subtract(StayFrom).TotalDays) + 1;

    public (bool valid, string fromSpan, string toSpan, string comment) ValidStayDates(DateTime today) =>
        StayFrom.IsMin() && StayFrom.IsMax()
            ? (true, DateConst.SpanActive, DateConst.SpanActive, string.Empty)  // Ignore if Min-Max - Stay any time

            : StayFrom >= StayTo
                ? (false, DateConst.SpanError, DateConst.SpanError, DateConst.FromAfterTo) // ERROR

                : StayTo <= today
                    ? (true, DateConst.SpanNormal, DateConst.SpanPast, DateConst.StayToPast) // Past. Can't use

                    : StayFrom > today
                        ? (true, DateConst.SpanFuture, DateConst.SpanFuture, DateConst.StayFuture) // Future - Not yet

                        : today > StayFrom && today <= StayTo
                            ? (true, DateConst.SpanActive, DateConst.SpanActive, DateConst.StayActive) // Active dates

                            : (true, DateConst.SpanNormal, DateConst.SpanNormal, string.Empty);
}