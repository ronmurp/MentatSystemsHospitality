namespace Msh.Opera.Ows.Models.AvailabilityResponses;

/// <summary>
/// Returned in a detailed availability request
/// </summary>
public class OwsRoomStayDetail
{
	public string? RatePlanCode { get; set; }
	public string? RoomTypeCode { get; set; }
	public List<OwsRoomRateDetail> OwsRoomRates { get; set; } = [];
	public OwsExpectedCharges? ExpectedCharges { get; set; }
}