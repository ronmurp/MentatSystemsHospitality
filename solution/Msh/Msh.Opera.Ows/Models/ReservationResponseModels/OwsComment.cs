namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsComment
{
	public string? Text { get; set; }
	public bool GuestViewable { get; set; } = true;
	public string CommentId { get; set; } = string.Empty;
}