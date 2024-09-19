using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsRoomRateRes
{
	public DateTime EffectiveDate { get; set; }
	public SoapAmount? Rate { get; set; }
	public string? RoomTypeCode { get; set; }
	public string? RatePlanCode { get; set; }
}