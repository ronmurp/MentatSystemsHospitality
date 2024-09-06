namespace Msh.Common.Attributes;

/// <summary>
/// A class that describes information related to a property on a class.
/// For use in javascript.
/// </summary>
public class PropertyValues
{
	/// <summary>
	/// Name of the property
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// String representation of the property's type, example: System.Boolean 
	/// </summary>
	public string DataType { get; set; } = string.Empty;

	/// <summary>
	/// An additional category that determines the type of editor to use
	/// </summary>
	/// <remarks>
	/// System.String - Text input (default), TextArea, Html (e.g., Monaco editor),
	/// See ConstCom.PropCat 
	/// </remarks>
	public string Category { get; set; } = string.Empty;
}