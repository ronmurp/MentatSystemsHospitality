namespace Msh.Common.Models.OwsCommon;

/// <summary>
/// Possible customer types
/// </summary>
/// <remarks>
/// Related to <c>AvailabilityMode</c>.
/// </remarks>
public enum CustomerTypes
{
	PublicBooking,  // Any customer, including offers.
	Promotion,      // A customer that is using a specific promotion code
	Company,        // May be Company or corporate Availability request?
	FitAgent        // FIT Agent
}