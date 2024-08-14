namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// The mode of one-time operation
/// </summary>
public enum OneTimeMode
{
    None,           // Default - discounts are not treated as one-time offers
    Code,           // Requires a specific discount code (that might need to be hashed)
    Reservation,    // Validates a previous reservation
    Login,          // Validates using a login,
    Email           // Validates using an email
}