using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models.AvailabilityResponses;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsRoomStayRes
{
	// Todo - OwsRoomStayRes - Under what circumstances would OWS ever return multiple?
	/// <summary>
	/// 
	/// </summary>
	public List<OwsRatePlanRes> RatePlans { get; set; } = [];

	public OwsRoomTypeRes? RoomType { get; set; }
	public List<OwsRoomRateRes> RoomRates { get; set; } = [];
	public bool GuestCountsIsPerRoom { get; set; }
	public List<OwsGuestCount> GuestCounts { get; set; } = [];
	public DateTime Arrive { get; set; }
	public DateTime Depart { get; set; }
	public string? HotelCode { get; set; }
	public string? ChainCode { get; set; }
	public SoapAmount? Total { get; set; }
	public List<OwsComment> Comments { get; set; } = [];
	public List<OwsPackageRes> Packages { get; set; } = [];
	public OwsExpectedCharges? ExpectedCharges { get; set; }
	public OwsGuarantee? Guarantee { get; set; }
	public List<OwsPaymentAccepted> PaymentsAccepted { get; set; } = [];
}