namespace Msh.Pay.FreedomPay.Models.Configuration;

public class PaymentTypeItem
{
    public string PaymentTypeId { get; set; } = string.Empty;

    public bool Enabled { get; set; }

    public string PaymentType { get; set; } = string.Empty;

    public string RequestType { get; set; } = string.Empty;

    public string Intent { get; set; } = string.Empty;

}