namespace Msh.Pay.FreedomPay.Models.Configuration;

public class PayPalConfig
{
    /// <summary>
    /// 1 - Unrestricted - Can accept pending alternate payment methods (APMs) where the final decision happens later.
    /// Note: Merchants must create an Hpc.WebhookUrl for these instances and provide it to FreedomPay’s Boarding Team.
    /// When the payment has finally been accepted, FP will send a success notification to this URL informing the integrator
    /// that the payment has completed successfully.Refer to Appendix A for more information.
    ///
    /// 2 - ImmediatePaymentRequired - Can only accept instant payments (this will return an error if an APM is used that is pending).
    /// </summary>
    public string PaymentMethodPayeePreferred { get; set; } = string.Empty;

    public string ShippingPreference { get; set; } = ConstFp.Paypal.Shipping.NoShipping;
}