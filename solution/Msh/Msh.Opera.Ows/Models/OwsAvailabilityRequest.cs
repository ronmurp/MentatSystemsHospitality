using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models;

/// <summary>
/// OWS Request data being passed in to general and detailed requests
/// </summary>
public class OwsAvailabilityRequest: OwsBaseRequest
{
	public AvailabilityMode AvailabilityMode { get; set; } = AvailabilityMode.Standard;
	public CustomerTypes CustomerType { get; set; } = CustomerTypes.PublicBooking;

	public int RoomIndex { get; set; }

	/// <summary>
	/// PromotionCode, CompanyCode, FIT Agent Search Code
	/// </summary>
	public string? SearchCode { get; set; }
       
	public int NumberOfRooms { get; set; } = 1;

	public int Adults { get; set; } = 1;
	public int Children { get; set; } = 0;
	public int Infants { get; set; } = 0;

	public int ChildCount => Children + Infants;
	public int GuestCount => Adults + ChildCount;

	/// <summary>
	/// For packages request
	/// </summary>
	public bool IncludeRestricted { get; set; }

	/// <summary>
	/// For detailed availability
	/// </summary>
	public string? RoomTypeCode { get; set; }

	/// <summary>
	/// For detailed availability
	/// </summary>
	public string? RatePlanCode { get; set; }
}