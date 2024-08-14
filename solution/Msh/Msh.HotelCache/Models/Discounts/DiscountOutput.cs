namespace Msh.HotelCache.Models.Discounts;

public class DiscountOutput
{
    // The amount being discounted
    public decimal Total { get; set; }

    public bool DiscountActive { get; set; }
    public DiscountTypes DiscountType { get; set; }
    public decimal DiscountRate { get; set; }
    public int Discount { get; set; }
    public decimal DiscountOff { get; set; }
}