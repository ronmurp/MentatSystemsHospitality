namespace Msh.HotelCache.Models.Discounts;

public enum BookingStatus
{
    Any = -1,
    Reserved,
    Prospect,
    NoShow,
    Canceled,
    InHouse,
    CheckedOut,
    Changed,
    WaitListed,
    PreCheckedIn,
    DueOut,
}