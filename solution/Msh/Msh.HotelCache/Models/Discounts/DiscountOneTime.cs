namespace Msh.HotelCache.Models.Discounts;

public class DiscountOneTime
{
    public OneTimeMode Mode { get; set; } = OneTimeMode.None;
    public OneTimeHashVersion HashMethod { get; set; } = OneTimeHashVersion.None;
    public bool RequiresReservationId { get; set; }
    public bool RequiresResvId { get; set; }
    public bool RequiresProfileId { get; set; }
    public bool RequiresEmail { get; set; }
    public bool RequiresLogin { get; set; }
    public bool RequiresPastDepart { get; set; }

    public BookingStatus BookingStatus { get; set; } = BookingStatus.Any;

    public bool AllowRollover { get; set; }
    public bool UpdateBooker { get; set; }
    public int ExpireCount { get; set; } = 0;
    public ExpireCountMode ExpireCountMode { get; set; } = ExpireCountMode.None;
}