using Msh.Common.Models.BaseModels;

namespace Msh.Common.Models.OwsCommon;

/// <summary>
/// A profile retrieved from OWS
/// </summary>
public class OwsProfile
{
	public string? ProfileId { get; set; }
	public string? CustomerType { get; set; }
	public string? CompanyName { get; set; }

	public string? Title { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }

	public BaseAddress Address { get; set; } = new BaseAddress();

	public List<OwsProfilePhone> Phones { get; set; } = new List<OwsProfilePhone>();

	public string? Email { get; set; }
}