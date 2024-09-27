namespace Msh.HotelCache.Models.Companies;

/// <summary>
/// The payment option depends primarily on the type of booker
/// </summary>
public enum PayOptions
{
	OptionsAll,      // They get to pick which option to use
	OnDeparture,    // Guest pays on departure
	ByCredit,       // Pays by credit/debit card
	BillBack,       // Customer (Company) will be billed

	Deposit         // Public bookings already have full/deposit options configure. This applies to FIT Agents
}