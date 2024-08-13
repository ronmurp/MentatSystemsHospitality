namespace Msh.Pay.FreedomPay.Models.Configuration;

public class ThreeDSecure
{
    /// <summary>
    /// Send 3DS data - or don't if false
    /// </summary>
    public bool Send3ds { get; set; }
    public bool Enabled { get; set; }
    public bool SoftDeclineEnabled { get; set; }
    public bool AutoMapToFreewayRequest { get; set; }
}