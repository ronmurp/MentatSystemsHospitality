namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// mimics Micros reservation service auto generated UniqueId 
/// </summary>
public class OwsUniqueId
{
	public OwsUniqueIdType Type { get; set; }
	public string? Source { get; set; }
	public string? Value { get; set; }
}