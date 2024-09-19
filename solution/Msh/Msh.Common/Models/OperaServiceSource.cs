namespace Msh.Common.Models;

/// <summary>
/// The type of Opera web service to use
/// </summary>
public enum OperaServiceSource
{
	/// <summary>
	/// The system used in March 2022 and earlier - asmx service
	/// </summary>
	OwsSystem51,

	/// <summary>
	/// Opera Cloud - April? 2022
	/// </summary>
	OwsCloud51
}