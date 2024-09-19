namespace Msh.Opera.Ows.Models;

public class OgEnvelope
{
	public DateTime TimeStampCreated { get; set; }

	public DateTime TimeStampExpires { get; set; }

	public string? ElhUserId { get; set; }

	public string? Password { get; set; }

	public string? TransactionId { get; set; }

	public string? HotelCode { get; set; }

}