namespace Msh.Opera.Ows.Models;

public class LovQueryResponse : OwsBaseResponse
{
	public OwsBusinessDate? OwsBusinessDate { get; set; }
	public List<OwsCountry> Countries { get; set; } = [];
}