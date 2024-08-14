namespace Msh.HotelCache.Models.Discounts;

public class DiscountCodesConfig
{
    public bool EnableDiscounts { get; set; }

    /// <summary>
    /// If true, shows the discount box on payment page always
    /// If false, shows it only if a discount code has been used in the search
    /// </summary>
    public bool EnableBox { get; set; }

    /// <summary>
    /// Default expire count for expiring discounts - each discount can have its own.
    /// </summary>
    public int ExpireCount { get; set; } = 8;
    public ExpireCountMode ExpireCountMode { get; set; } = ExpireCountMode.Weeks;
}