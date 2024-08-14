namespace Msh.HotelCache.Models.Discounts;

/// <summary>
/// The type of discount, in DiscountCodes.xml
/// </summary>
public enum DiscountTypes
{
    None, // Disabled
    Percent,            // The Discount property is a fraction representing a percent: 10 => 10%
    Nights,             // The Discount property represents the number of nights discounted
    Rooms,              // The Discount property represents the number of rooms discounted - based on average price
    RoomsLowest,        // The Discount property represents the number of rooms discounted - based on lowest price
    Cash,               // The Discount property represents a whole number of pounds discount
    RatePlan,            // The discount is contained entirely within the rate plan,
    FitAgent
}