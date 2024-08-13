using System.Text.Json.Serialization;
using Msh.Common.Constants;
using Msh.Common.ExtensionMethods;

namespace Msh.Common.Models.Dates;

public class OfferDate : StayDates
{
    public bool EnabledBookDates { get; set; } = false;

    public DateTime BookFrom { get; set; } = DateTime.MinValue;

    public DateTime BookTo { get; set; } = DateTime.MaxValue;

    [JsonIgnore]
    public string BookFromString => BookFrom.ToString(DateConst.DateFormat);

    [JsonIgnore]
    public string BookToString => BookTo.ToString(DateConst.DateFormat);


    public (bool valid, string fromSpan, string toSpan, string comment) ValidBookDates(DateTime today) =>
        !EnabledBookDates
            ? (true, DateConst.SpanInActive, DateConst.SpanInActive, string.Empty) // Ignore if not using book dates

            : BookFrom.IsMin() && BookTo.IsMax()
                ? (true, DateConst.SpanActive, DateConst.SpanActive, string.Empty)  // Ignore if Min-Max - Book any time

                : BookFrom >= BookTo
                    ? (false, DateConst.SpanError, DateConst.SpanError, DateConst.FromAfterTo)

                    : BookFrom >= StayTo
                        ? (false, DateConst.SpanError, DateConst.SpanError, DateConst.BookTooLate)

                        : BookTo >= StayTo
                            ? (true, DateConst.SpanNormal, DateConst.SpanError, DateConst.BookToIsLate)

                            : BookTo < today
                                ? (true, DateConst.SpanNormal, DateConst.SpanError, DateConst.BookToPast)

                                : (true, DateConst.SpanNormal, DateConst.SpanNormal, string.Empty);

}