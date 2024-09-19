using Msh.Opera.Ows.Models.ReservationResponseModels;

namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Used to add comments to a booking. Example: linking rooms in multiple room booking
/// </summary>
public class OwsAddBookingCommentRequest : OwsBaseRequest
{
	public string ReservationId { get; set; } = string.Empty;

	public List<OwsComment> Comments { get; set; } = [];
}