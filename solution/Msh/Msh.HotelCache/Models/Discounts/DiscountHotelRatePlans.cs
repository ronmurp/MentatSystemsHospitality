namespace Msh.HotelCache.Models.Discounts;

public class DiscountHotelRatePlans
{
    public string HotelCode { get; set; } = string.Empty;
    public bool HideNonDiscounts { get; set; }
    public List<DiscountRatePlan> RatePlans { get; set; } = new List<DiscountRatePlan>();
}