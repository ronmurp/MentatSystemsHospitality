namespace Msh.Common.Models.OwsCommon;

/// <summary>
/// A common structure for SOAP message components. See LinqXmlExtensionMethods.GetSoapAmount
/// </summary>
public class SoapAmount
{
	/// <summary>
	/// The amount value
	/// </summary>
	public decimal Amount { get; set; }
	public string? CurrencyCode { get; set; } = "GBP";

	/// <summary>
	/// The number of decimal places included
	/// </summary>
	public int DecimalPlaces { get; set; } = 2;
}