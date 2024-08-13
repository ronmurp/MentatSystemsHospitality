namespace Msh.Pay.FreedomPay.Models.Configuration;

/// <summary>
/// A list of these forms a bank of messages, identified by Code,
/// and used in FpErrorCode.Use
/// </summary>
public class FpErrorCodeBank()
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}