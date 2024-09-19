using Msh.Common.Models.BaseModels;

namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Base address extended with phone and email.
/// </summary>
public class OwsReservationAddress : BaseAddress
{
	public string? Telephone { get; set; }
	public string? Email { get; set; }
}