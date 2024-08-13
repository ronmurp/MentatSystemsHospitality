namespace Msh.Pay.FreedomPay.Models.Configuration;

/// <summary>
/// Error codes (ReasonCode). A list of these allows a ReasonCode to look up an associated message.
/// </summary>
public class FpErrorCode()
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Action level: 0=stay on page; 1=go to booking support, 2 = serious.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// If this is not empty, get the Message from the FpErrorCodeBank.Message
    /// for the corresponding FpErrorCodeBank.
    /// </summary>
    public string Use { get; set; } = string.Empty;
}