using Msh.Common.Models;
using Msh.Opera.Ows.Models.ReservationResponseModels;

namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Details of a guest to include in a reservation. Also returned by OWS.
/// </summary>
public class OwsReservationGuest
{
	public string?  Title { get; set; }
	public string?  FirstName { get; set; }
	public string?  LastName { get; set; }

	/// <summary>
	/// Only for the first guest or booker of the first room
	/// </summary>
	public OwsReservationAddress? Address { get; set; }

	public OwsUniqueId? ProfileId { get; set; }
	public List<OwsPhone> Phones { get; set; } = [];
	public List<OwsEmail> Emails { get; set; } = [];
	public int Age { get; set; }

	public bool CotRequired { get; set; }
	public ContactTypes ContactType { get; set; }
}