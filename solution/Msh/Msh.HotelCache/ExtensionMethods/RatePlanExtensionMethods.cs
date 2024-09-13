using Msh.Common.Constants;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.ExtensionMethods;

public static class RatePlanExtensionMethods
{
	public static string StayFromString(this RoomRatePlan ratePlan) => ratePlan.StayFrom.ToString(DateConst.DateFormat);
	public static string StayToString(this RoomRatePlan ratePlan) => ratePlan.StayTo.ToString(DateConst.DateFormat);

	public static (bool valid, string fromSpan, string toSpan, string comment)
		ValidBookDates(this RoomRatePlan rp, DateOnly today) =>
		!rp.HasBookDates
			? (true, DateConst.SpanInActive, DateConst.SpanInActive, string.Empty) // Ignore if not using book dates

			: rp.BookFrom.IsMin() && rp.BookTo.IsMax()
				? (true, DateConst.SpanActive, DateConst.SpanActive, string.Empty)  // Ignore if Min-Max - Book any time

				: rp.BookFrom >= rp.BookTo
					? (false, DateConst.SpanError, DateConst.SpanError, DateConst.FromAfterTo)

					: rp.BookFrom >= rp.StayTo
						? (false, DateConst.SpanError, DateConst.SpanError, DateConst.BookTooLate)

						: rp.BookTo >= rp.StayTo
							? (true, DateConst.SpanNormal, DateConst.SpanError, DateConst.BookToIsLate)

							: rp.BookTo < today
								? (true, DateConst.SpanNormal, DateConst.SpanError, DateConst.BookToPast)

								: (true, DateConst.SpanNormal, DateConst.SpanNormal, string.Empty);
}