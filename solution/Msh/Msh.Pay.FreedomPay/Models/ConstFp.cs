namespace Msh.Pay.FreedomPay.Models;

public static class ConstFp
{
    public const string FpConfig = "FpConfig";
    public const string FpErrorCodeBank = "FpErrorCodeBank";
    public const string FpErrorCode = "FpErrorCode";
    public const string FpPaymentType = "FpPaymentType";
    public const string FpKmapConfig = "FpKmapConfig";

    public static class PaymentType
    {

        public const string Card = "Card"; // (Default) 1
        public const string CardOnFile = "CardOnFile"; // 2
        public const string GiftCard = "GiftCard"; // 3
        public const string RewardCard = "RewardCard"; // 4
        public const string GooglePay = "GooglePay";
        public const string ApplePay = "ApplePay";
        public const string PayPal = "PayPal";
        public const string PayByBank = "PayByBank";

        public const string PrivateLabelCard = "PrivateLabelCard"; // 7

        public const string Bitcoin = "Bitcoin";
        public const string CoinCorner = "CoinCorner";
    }
    public static class RequestType
    {
        public static readonly string CardPayment = "CardPayment";
        public static readonly string GiftCardPayment = "GiftCardPayment";
        public static readonly string RewardCardPayment = "RewardCardPayment";
        public static readonly string TokenPayment = "TokenPayment";
        public static readonly string GooglePayPayment = "GooglePayPayment";
        public static readonly string ApplePayPayment = "ApplePayPayment";
        public static readonly string PrivateLabelCardPayment = "PrivateLabelCardPayment";
        public static readonly string PayPalPayment = "PayPalPayment";
        public static readonly string PayByBankPayment = "PayByBankPayment";
        public static readonly string CardPaymentTokenization = "CardPaymentTokenization";
        public static readonly string Tokenization = "Tokenization";
    }

    public static class ButtonType
    {
        public static readonly string Buy = "Buy"; // 1 Buy
        public static readonly string Donate = "Donate"; // 2 Donate
        public static readonly string Next = "Next"; // 3 Next
        public static readonly string Pay = "Pay"; // (Default) 4 Pay
        public static readonly string Save = "Save"; // 5 Save
        public static readonly string BuyNow = "BuyNow"; // 6 Buy Now
        public static readonly string PayNow = "PayNow"; // 7 Pay Now
        public static readonly string PayWithCard = "PayWithCard"; // 8 Pay with Card
        public static readonly string CompleteReservation = "CompleteReservation"; // 9 Complete Your Reservation
    }

    public static class LegalClick
    {
        public static readonly string Terms = "terms";
        public static readonly string Privacy = "privacy";
    }

    public static class LegalTextType
    {
        public static readonly string DynamicWithCheckbox = "DynamicWithCheckbox"; // 1 (default) “By checking this box and clicking“Pay with Card,” you agree to the Terms and Conditions …”
        public static readonly string DynamicWithoutCheckbox = "DynamicWithoutCheckbox"; // 2 “By clicking “Pay with Card,” you agree to the Terms and Conditions …”

        // When using the third-party payment method for 3 and 4, “Pay with Card” will be replaced with Apple Pay, Google Pay, or PayPal.
        public static readonly string DynamicThirdPartyWithCheckbox = "DynamicThirdPartyWithCheckbox"; // 3 “By checking this box and submitting your order using Pay with Card, you agree to the Terms and Conditions…”
        public static readonly string DynamicThirdPartyWithoutCheckbox = "DynamicThirdPartyWithoutCheckbox"; // 4 “By submitting your order using Pay with Card, you agree to the Terms and Conditions…”
    }

    //  These fields are passed in the init request in slightly different ways depending on which version of HPC the merchant is running
    // These are for v1.4

    public static class FieldType
    {
        public static class Card
        {
            public static class LabelType
            {
                public static readonly string Default = "Default"; // 1 
                public static readonly string Required = "Required";
            }

            public static class Placeholder
            {
                public static readonly string Blank = "Blank"; // 1 
                public static readonly string FourOnes = "4111111111111111"; // (Default) 2 4111111111111111
                public static readonly string EnterCardNumber = "EnterCardNumber"; // 3 Enter Card Number
            }
        }

        public static class Expire
        {
            public static class LabelType
            {
                public static readonly string Default = "Default"; // 1 Expiration Date
                public static readonly string Required = "Required"; // 2 Expiration Date (required)
                public static readonly string IfPresent = "IfPresent"; // 3 Expiration Date (if present)
                public static readonly string Optional = "Optional"; // 4 Expiration Date (optional)
            }

            public static class Placeholder
            {
                public static readonly string Blank = "Blank"; // 1
                public static readonly string MonthYear = "MM/YY"; // (Default) 2 MM/YY	
            }
        }

        public static class Security
        {
            public static class LabelType
            {
                public static readonly string Default = "Default"; // 1 Security Code
                public static readonly string Required = "Required"; // 2 Security Code (required)
                public static readonly string IfPresent = "IfPresent"; // 3 Security Code (if present)
                public static readonly string Optional = "Optional"; // 4 Security Code (optional)
                public static readonly string PinDefault = "PinDefault"; // 6 PIN/Security Code
            }

            /// <summary>
            /// 
            /// </summary>
            /// <remarks>
            ///  The Pin Default (LabelType) and Pin (PlaceholderType) values only apply
            ///  to gift card payments. If gift cards are not supported and these values are passed
            ///  in the init request, the default values (LabelType: Default; PlaceholderType:123)
            /// </remarks>
            public static class Placeholder
            {
                public static readonly string Blank = "Blank"; // 1 none
                public static readonly string N123 = "123"; // 2 (Default)  123
                public static readonly string Cvc = "Cvc"; // 3 CVC
                public static readonly string Cvv = "Cvv"; // 4 CVV
                public static readonly string CvcNumber = "CvcNumber"; // 5 CVC Number
                public static readonly string CvvNumber = "CvvNumber"; // 6 CVV Number
                public static readonly string Pin = "Pin"; // 7 PIN/Security Code
            }

            /// <summary>
            /// 
            /// </summary>
            /// <remarks>
            /// Use OptionalExplicit for payments where the customer does not have their security code.
            /// A checkbox will appear, and the customer can select this box to indicate their intent to
            /// leave this field empty on the Payment Page.
            /// </remarks>
            public class ValidationType
            {
                public static readonly string NotApplicable = "NotApplicable"; // 1 Use this value to ensure that SecurityCode does not appear at all on the Payment Page.
                public static readonly string Optional = "Optional"; // 2 Use this to make SecurityCode a field that the customer has the option to fill out or leave blank
                public static readonly string Required = "Required"; // 3 Use this to make SecurityCode a required field that the customer must enter
                public static readonly string OptionalExplicit = "OptionalExplicit"; // 4 Use this for payments where the customer does not have their security code.
            }
        }

        public static class PostalCode
        {
            public static class LabelType
            {
                public static readonly string Default = "Default"; // 1 
                public static readonly string Required = "Required";
            }

            public static class Placeholder
            {
                public static readonly string Blank = "Blank"; // 1
                public static readonly string N11111 = "11111"; //  2 (Default) 11111
                public static readonly string EnterPostalCode = "EnterPostalCode"; // 3 Enter Postal Code

            }

            /// <summary>
            /// 
            /// </summary>
            /// <remarks>
            /// 1 Use this to make PostalCode not appear at all on the Payment Page. If not added, it defaults to Required.
            /// In C# use UseIt (null) if it is required
            /// </remarks>
            public static class ValidationType
            {
                public static string UseIt = null;
                public static readonly string NotApplicable = "NotApplicable"; // 1 Use this to make PostalCode not appear at all on the Payment Page. If not added, it defaults to Required.

            }
        }

    }


    public static class CultureCode
    {
        public static string Germany = "de-DE"; // German/Germany
        public static string EnglishUS = "en-US"; //  (default) English/United States
        public static string Spain = "es-ES"; //  Spanish/Spain
        public static string FrenchCanada = "fr-CA"; //  French/Canada
        public static string FrenchFrance = "fr-FR"; //  French/France
        public static string Italy = "it-IT"; //  Italian/Italy
    }

    /// <summary>
    /// ValidationMessageType - the message that is displayed to the user if they don’t enter in any information
    /// </summary>
    /// <remarks>
    /// ValidationMessageType (string or numeric value – this refers to the message that is
    /// displayed to the user if they don’t enter in any information in the IFrame or enter in the wrong
    /// information. Refer to Section 2.2 for more information).
    /// </remarks>
    public static class ValidationMessageType
    {
        public static readonly string None = "None"; // 1
        public static readonly string Feedback = "Feedback"; // (Default) 2
        public static readonly string Tooltip = "Tooltip"; // 3
    }

    public static class Paypal
    {
        /// <summary>
        /// Intent (string or numeric value – this Intent drives the final payment request. If
        /// a merchant sets this to Authorization, this means they can only perform an
        /// Auth during the payment request; if set to Capture, this means they can only
        /// perform a Capture 
        /// </summary>
        public static class Intent
        {
            /// <summary>
            /// Merchants cannot set ‘Intent’ to Auth and then try to Capture during the HPC
            /// payment call. If this is set to Auth, merchants must perform a Capture later
            /// via FreedomPay’s Enterprise Portal or Direct-to-FreeWay.  
            /// </summary>
            public static readonly string Authorization = "Authorization";

            /// <summary>
            /// A Capture is basically a sale (i.e., an Authorization AND a Capture at once). A
            /// Capture does not require any follow-on requests (except for refunds – see Section
            /// 2.6 for more information). 
            /// </summary>
            public static readonly string Capture = "Capture"; // Default
        }

        /// <summary>
        /// PaymentMethodPayeePreferred (string or numeric value – this helps guide
        /// merchants to determine if they want to ONLY accept “instant” payments OR if they
        /// want to also accept pending payments where the final decision/capture happens
        /// later)
        /// </summary>
        public static class PayeePreferred
        {
            /// <summary>
            /// Can accept pending alternate payment methods (APMs) where the final
            /// decision happens later. Note: Merchants must create an
            /// Hpc.WebhookUrl for these instances and provide it to FreedomPay’s
            /// Boarding Team. When the payment has finally been accepted, FP will send a
            /// success notification to this URL informing the integrator that the
            /// payment has completed successfully. Refer to Appendix A for more information 
            /// </summary>
            public static readonly string Unrestricted = "Unrestricted"; // 1 (Default)

            /// <summary>
            /// Can only accept instant payments (this will return an error if an APM is used
            /// that is pending). 
            /// </summary>
            public static readonly string ImmediatePaymentRequired = "ImmediatePaymentRequired"; // 2
        }


        /// <summary>
        /// 
        /// </summary>
        public static class Shipping
        {
            /// <summary>
            /// This will retrieve the customer’s shipping address from whatever PayPal has on file
            /// (i.e., whatever the customer selects when they select their address from PayPal).
            /// </summary>
            public static readonly string GetFromFile = "GetFromFile"; // 1 Default

            /// <summary>
            /// Turn off requiring ShippingAddress altogether
            /// </summary>
            public static readonly string NoShipping = "NoShipping"; // 2
        }
    }

    public class CardIcon
    {
        // Generic card icon is display by default or if no matches are found based on BIN range.
        public static readonly string Dynamic = "Dynamic"; // (Default) 1 Displays card icon based on matching BIN range. 
        public static readonly string Fixed = "Fixed"; // 2 Always displays generic card icon
        public static readonly string Hidden = "Hidden"; // 3 No icon
    }

    public static class WorkflowType
    {
        public static readonly string Standard = "Standard"; // (Default) 1 Generates single payment key for single use.
        public static readonly string VerifyAuth = "VerifyAuth"; // 2 Generates multiple payment keys for multiuse.
    }

    public static class ReasonCode
    {
        public const string Default = "Default";

        /// <summary>
        /// Utility ReasonCode for undetermined rejection
        /// </summary>
        public const string R000 = "000";

        /// <summary>
        /// ACCEPT ReasonCode - but there may be AVS/CVV errors
        /// </summary>
        public const string R100 = "100";

        /// <summary>
        /// One or more required fields missing from the request. Consult the invalidFieldsEntry logged in the main error log.
        /// </summary>
        public const string R101 = "101";

        /// <summary>
        /// One or more required fields in the request contain invalid data. Consult the invalidFieldsEntry logged in the main error log.
        /// </summary>
        public const string R102 = "102";

        /// <summary>
        /// An invalid combination of services was requested.
        /// </summary>
        public const string R103 = "103";

        /// <summary>
        /// Duplicate transaction. WBS should protect against this.
        /// </summary>
        public const string R104 = "104";

        /// <summary>
        /// Issue occurred processing the request - application error. Do not retry the transaction. Contact FreedomPay.
        /// </summary>
        public const string R149 = "149";

        /// <summary>
        /// Issue occurred processing the request. Contact FreedomPay.
        /// </summary>
        public const string R150 = "150";

        /// <summary>
        /// An internal timeout occurred while processing the request. Try again.
        /// </summary>
        public const string R151 = "151";

        /// <summary>
        /// An internal error occurred while communicating with the card processor. Contact FreedomPay.
        /// </summary>
        public const string R152 = "152";

        /// <summary>
        /// Unable to communicate with the card processor. Try again.
        /// </summary>
        public const string R153 = "153";

        /// <summary>
        /// Invalid card processor configuration. Contact FreedomPay.
        /// </summary>
        public const string R154 = "154";

        /// <summary>
        /// Internal communication error. Try again.
        /// </summary>
        public const string R155 = "155";

        /// <summary>
        /// Business date required. If this occurs WBS may have a problem. Investigate.
        /// </summary>
        public const string R161 = "161";

        /// <summary>
        /// Business date is earlier than the most recent date.
        /// </summary>
        public const string R162 = "162";

        /// <summary>
        /// Call issuing bank for authorsation.
        /// </summary>
        public const string R201 = "201";

        /// <summary>
        /// Expired card (or mismatched expiry date provided)
        /// </summary>
        public const string R202 = "202";

        /// <summary>
        /// Declines by the issuing bank. No reason given.
        /// </summary>
        public const string R203 = "203";

        /// <summary>
        /// Insufficient funds.
        /// </summary>
        public const string R204 = "204";

        /// <summary>
        /// Lost or stolen card.
        /// </summary>
        public const string R205 = "205";

        /// <summary>
        /// Stolen card.
        /// </summary>
        public const string R206 = "206";

        /// <summary>
        /// Issuing bank unavailable to authorize request. Try another card.
        /// </summary>
        public const string R207 = "207";

        /// <summary>
        /// The card is not active or not eligible for this type of transaction. Try again.
        /// </summary>
        public const string R208 = "208";



        public const string R216 = "216";
        public const string R217 = "217";

        /// <summary>
        /// Merchant configuration error. Contact FreedomPay.
        /// </summary>
        public const string R229 = "229";

        /// <summary>
        /// Invalid account number.
        /// </summary>
        public const string R231 = "231";

        /// <summary>
        /// Card Type not enabled for merchant.
        /// </summary>
        public const string R232 = "232";

        /// <summary>
        /// Processor rejected the transaction due to an issue with the request.
        /// </summary>
        public const string R233 = "233";

        /// <summary>
        /// Invalid merchant credentials. Contact FreedomPay.
        /// </summary>
        public const string R234 = "234";

        /// <summary>
        /// Processor reported an error while attempting to process the request. Contact FreedomPay.
        /// </summary>
        public const string R237 = "237";


        /// <summary>
        /// The FreedomPay session key does not match
        /// </summary>
        public const string ExpiredFpSession = "ExpiredFpSession";

        /// <summary>
        /// Lost FreedomPay Wbs session data
        /// </summary>
        public const string LostSession = "LostSession";

        /// <summary>
        /// A successful submit payments has already returned with success
        /// </summary>
        public const string RepeatPayment = "RepeatPayment";

        /// <summary>
        /// Unspecified iframe error
        /// </summary>
        public const string IFrameError = "IFrameError";

        /// <summary>
        /// Bad transition in the state machine
        /// </summary>
        public const string BadStateTransition = "BadStateTransition";

        /// <summary>
        /// A submit payment is already in progress
        /// </summary>
        public const string PendingPayment = "PendingPayment";


        /// <summary>
        /// The maximum number of submission attempts has been made
        /// </summary>
        public const string MaxAttempts = "MaxAttempts";

        /// <summary>
        /// A timeout reverse has been performed ... and succeeded
        /// </summary>
        public const string TimeoutReverse = "TimeoutReverse";

        /// <summary>
        /// ELH policy not to use reverse. See conversations with Harvie 20/9/2022
        /// </summary>
        public const string TimeoutNoReverse = "TimeoutNoReverse";

        public const string InvalidPaymentTypeId = "InvalidPaymentTypeId";

        public const string AvsCvv = "AvsCvv";

        /// <summary>
        /// The response from the payment submit did not contain valid json
        /// </summary>
        public const string NoJsonSubmitResponse = "NoJsonSubmitResponse";

        /// <summary>
        /// Unspecified unexpected error - the details will be in the logged error
        /// </summary>
        public const string Unexpected = "Unexpected";

        public const string NullReference = "NullReference";

        public const string InvalidPrice = "InvalidPrice";

        public const string BadRequest = "BadRequest";
    }

}