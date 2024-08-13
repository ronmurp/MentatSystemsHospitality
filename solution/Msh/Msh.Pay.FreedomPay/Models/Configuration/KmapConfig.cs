namespace Msh.Pay.FreedomPay.Models.Configuration;

/// <summary>
/// A subset of payment config data, identified by a key. A component of a list
/// </summary>
public class KmapConfig
{
    public string Key { get; set; } = string.Empty;

    public string TerminalId { get; set; } = string.Empty;

    public string StoreId { get; set; } = string.Empty;
}