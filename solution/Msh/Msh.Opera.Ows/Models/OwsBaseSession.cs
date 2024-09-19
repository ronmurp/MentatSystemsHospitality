using Msh.Common.Services;

namespace Msh.Opera.Ows.Models;

/// <summary>
/// Allows OWS calls to log SOAP with part of sessionId
/// </summary>
public class OwsBaseSession
{
	public string? HotelCode { get; set; }
	public string? ChainCode { get; set; }

	public string TransactionId { get; set; } = CommonHelper.NewGuidN();
	public string SessionKey { get; set; } = string.Empty;
}