namespace Msh.Opera.Ows.Models.ReservationRequestModels;

public class OwsPackageRequest : OwsBaseRequest
{
	public string? ReservationId { get; set; }

	public int RoomIndex { get; set; }
	public int Quantity { get; set; }
	public string? PackageCode { get; set; }
}