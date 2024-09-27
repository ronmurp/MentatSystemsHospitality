namespace Msh.HotelCache.Models.Greens;

/// <summary>
/// The % of the final bill that will be added as a carbon offset. Guest can opt out.
/// </summary>
public class CarbonOffset
{
	public decimal Percent { get; set; } = 0M;
	public decimal MinimumCharge { get; set; } = 0M; // e.g. £0.50 - pounds sterling
	public bool Enabled { get; set; }

	/// <summary>
	/// Not sure if this is used
	/// </summary>
	public bool DefaultPay { get; set; }

	/// <summary>
	/// The prompt that appears with the checkbox that the user clicks to accept this option
	/// </summary>
	/// <remarks>
	/// The following html can include the following variables that will be replaced at runtime:
	/// {Percent} - The percent value above
	/// {MinimumCharge} - The MinimumCharge value above
	/// {Value} - The calculated value, using the booking total x Percent, or MinimumCharge, whichever is greatest.
	/// </remarks>
	public string? CheckboxPromptHtml { get; set; }

	/// <summary>
	/// The text that appears in confirmation emails and pages that explains the charge
	/// </summary>
	/// <remarks>
	/// The following html can include the following variables that will be replaced at runtime:
	/// {Percent} - The percent value above
	/// {MinimumCharge} - The MinimumCharge value above
	/// {Value} - The calculated value, using the booking total x Percent, or MinimumCharge, whichever is greatest.
	/// </remarks>
	public string? ConfirmationHtml { get; set; }

	/// <summary>
	/// The text passed to the booking system, Opera, when the user has accepted the charge
	/// </summary>
	public string? OperaAcceptedText { get; set; }

	/// <summary>
	/// The text passed to the booking system, Opera, when the user has declined the charge.
	/// This is sent only if the feature is enabled.
	/// </summary>
	public string? OperaDeclinedText { get; set; }

	public string? Notes { get; set; }
}