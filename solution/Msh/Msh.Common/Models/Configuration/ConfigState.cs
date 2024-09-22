using System.ComponentModel.DataAnnotations;

namespace Msh.Common.Models.Configuration;

/// <summary>
/// The state of a config record
/// </summary>
public class ConfigState
{
	/// <summary>
	/// The name is appended to other record types.
	/// </summary>
	[Display(Name="Code")]
	[StringLength(6, MinimumLength = 3)]
	public string Code { get; set; } = string.Empty;

	[Display(Name = "Description")]
	public string Description { get; set; } = string.Empty;
}