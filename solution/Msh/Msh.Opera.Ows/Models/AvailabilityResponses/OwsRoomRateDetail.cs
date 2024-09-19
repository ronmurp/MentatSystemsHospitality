using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.AvailabilityResponses;

/// <summary>
/// Returned in a detailed availability request
/// </summary>
public class OwsRoomRateDetail
{
	public string? RoomTypeCode { get; set; }
	public string? RatePlanCode { get; set; }

	public List<OwsDateRate> DayRates { get; set; } = [];
	public SoapAmount? Total { get; set; }
}