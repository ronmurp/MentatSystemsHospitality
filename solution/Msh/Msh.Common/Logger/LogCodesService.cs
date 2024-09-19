using System.Reflection;
using System.Text.Json;
using Msh.Common.Models;

namespace Msh.Common.Logger;

public class LogCodesService : ILogCodesService
{
	public List<LogCodesItem> GetLogCodes()
	{
		List<LogCodesItem> logCodes = new List<LogCodesItem>();

		foreach (FieldInfo field in typeof(LogCodes).GetFields())
		{
			var lc = new LogCodesItem
			{
				Name = field.Name,
				Code = field.GetRawConstantValue().ToString(),
				Enabled = true,

			};

			lc.Description = Lookup(lc.Code);

			logCodes.Add(lc);
		}

		return logCodes;
	}

	public List<LogCodesItem> LoadLogCodes(string appDataPath, string userId = "")
	{
		var list = GetLogCodes().OrderBy(c => c.Code).ToList();
		var dic = new Dictionary<string, string>();

		try
		{
			var path = Path.Combine(appDataPath, "System", "LogCodes.Json");
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);

				var savedCodes = JsonSerializer.Deserialize<List<LogCodesItem>>(json);


				foreach (var item in list)
				{
					if (dic.ContainsKey(item.Code))
					{
						//WbsLogger.InfoAdmin(LogCodes.AdminLogCodes, "", item, $"Duplicate Code {item.Code}");
					}
					else
					{
						dic.Add(item.Code, item.Code);
					}
					var savedCode = savedCodes?.FirstOrDefault(c => c.Code == item.Code) ?? null;
					if (savedCode != null)
					{
						item.Enabled = savedCode.Enabled;
					}
				}
			}


		}
		catch (Exception ex)
		{
			//WbsLogger.ErrorAdmin(LogCodes.AdminLogCodes, userId, ex, "");
		}

		return list;
	}

	public bool SaveLogCodes(string appDataPath, List<LogCodesItem> list, string userId)
	{
		try
		{
			var path = Path.Combine(appDataPath, "System", "LogCodes.Json");
			var json = JsonSerializer.Serialize(list);
			File.WriteAllText(path, json);

			return true;
		}
		catch (Exception ex)
		{
			//WbsLogger.ErrorAdmin(LogCodes.AdminLogCodes, userId, ex, "");
		}

		return false;
	}
	public string Lookup(string logCode)
	{

		switch (logCode)
		{
			case LogCodes.AdmCheckHotelCode: return "";
			case LogCodes.AdminAnalytics: return "";
			case LogCodes.AdminApiPasswordRequest: return "";
			case LogCodes.AdminApiPasswordReset: return "";
			case LogCodes.AdminApiPasswordValidate: return "";
			case LogCodes.AdminApiValidate: return "";
			case LogCodes.AdminBkReservationLogs: return "";
			case LogCodes.AdminCookieTrace: return "";
			case LogCodes.AdminDev: return "";
			case LogCodes.AdminEmails: return "";
			case LogCodes.AdminFit: return "";
			case LogCodes.AdminFitBanner: return "";
			case LogCodes.AdminFitRates: return "";
			case LogCodes.AdminFitUsers: return "";
			case LogCodes.AdminLogCodes: return "";
			case LogCodes.AdminLoginError: return "";
			case LogCodes.AdminLoginFail: return "";
			case LogCodes.AdminLoginOk: return "";
			case LogCodes.AdminLogs: return "";
			case LogCodes.AdminLogViewer: return "";
			case LogCodes.AdminMenuHelper: return "";
			case LogCodes.AdminPasswordLoad: return "";
			case LogCodes.AdminPayments: return "";
			case LogCodes.AdminPcBlockDates: return "";
			case LogCodes.AdminPcBookings: return "";
			case LogCodes.AdminPcCart: return "";
			case LogCodes.AdminPcCategories: return "";
			case LogCodes.AdminPcCats: return "";
			case LogCodes.AdminPcCrossSell: return "";
			case LogCodes.AdminPcDev: return "";
			case LogCodes.AdminPcDpt: return "";
			case LogCodes.AdminPcFacilities: return "";
			case LogCodes.AdminPcGlobal: return "";
			case LogCodes.AdminPcImages: return "";
			case LogCodes.AdminPcOp: return "";
			case LogCodes.AdminPcPublish: return "";
			case LogCodes.AdminPcRawData: return "";
			case LogCodes.AdminPcRawSearch: return "";
			case LogCodes.AdminPcResDel: return "";
			case LogCodes.AdminPcRooms: return "";
			case LogCodes.AdminPcRules: return "";
			case LogCodes.AdminPcTabItems: return "";
			case LogCodes.AdminPcTabs: return "";
			case LogCodes.AdminPcTypes: return "";
			case LogCodes.AdminProcessCommand: return "";
			case LogCodes.AdminPublish: return "";
			case LogCodes.AdminPublishData: return "";
			case LogCodes.AdminPushData: return "";
			case LogCodes.AdminRatePlans: return "";
			case LogCodes.AdminSaveEmail: return "Saving an email record to the logs. EmailLogging.SaveEmail.";
			case LogCodes.AdminSiteMessages: return "";
			case LogCodes.AdminStickyBanner: return "";
			case LogCodes.AdminTestGetIframe: return "";
			case LogCodes.AdminTestMaster: return "";
			case LogCodes.AdminTestTrap: return "";
			case LogCodes.AdminUpdate: return "";
			case LogCodes.AdminVouchers: return "";
			case LogCodes.AdminVouchersPublish: return "";
			case LogCodes.Analytics: return "";
			case LogCodes.AnalyticsLoad: return "";
			case LogCodes.ApiPayLog: return "";
			case LogCodes.ApiPayLogError: return "";
			case LogCodes.AppStart: return "The web site has been restarted: manually by development; after a site update; after IIS recycling.";
			case LogCodes.AvailPlans: return "";
			case LogCodes.AvailRooms: return "";
			case LogCodes.AvsCvvFail: return "";
			case LogCodes.BookByCard: return "Book by card: OK, Part Booked, Fail (see log) in PaymentSetupService";
			case LogCodes.BookingSupportPageLoad: return "";
			case LogCodes.BookNoCard: return "Booking for companies on account.";
			case LogCodes.CalcCheck: return "Checking calculations and recalculating in CalculateAmounts";
			case LogCodes.CalcInvalid: return "";
			case LogCodes.CaptchaError: return "Errors in the CaptchaService";
			case LogCodes.CcApiQrCode: return "";
			case LogCodes.CcApiStatus: return "";
			case LogCodes.CcCancel: return "In BitCoinController";
			case LogCodes.CcFail: return "In BitCoinController";
			case LogCodes.CcNotify: return "In BitCoinController - Notification From CoinCorner";
			case LogCodes.CcNotifyComplete: return "In BitCoinController - Notification From CoinCorner - Complete";
			case LogCodes.CcProgress: return "In BitCoinController";
			case LogCodes.CcQrCode: return "In BitCoinController";
			case LogCodes.CcStatusCheck: return "In BitCoinController - Every check. Can be disabled.";
			case LogCodes.CcSuccess: return "In BitCoinController";
			case LogCodes.ConfirmationPageLoad: return "";
			case LogCodes.DetailsPageLoad: return "";
			case LogCodes.EmailElh: return "";
			case LogCodes.EmailFilename: return "Getting text from an email file.";
			case LogCodes.EmailFrom: return "";
			case LogCodes.EmailLoad: return "";
			case LogCodes.EmailNotFound: return "PasswordService.SendEmailAsync for FIT";
			case LogCodes.EmailSave: return "";
			case LogCodes.Extras: return "";
			case LogCodes.FileAddItem: return "Adding a record to a json file - AddToJsonListAsync or AddListToJsonListAsync.";
			case LogCodes.FileDelete: return "Deleting a record in a json file - DeleteFromJsonListAsync.";
			case LogCodes.FileUpdate: return "Updating a record in a json file - UpdateToJsonListAsync.";
			case LogCodes.FitBookingSupportPageLoad: return "";
			case LogCodes.FitConfirmationPageLoad: return "";
			case LogCodes.FitDetailsPageLoad: return "";
			case LogCodes.FitGetAgent: return "Get FIT Agent data.";
			case LogCodes.FitPasswordToken: return "Verifying FIT password token.";
			case LogCodes.FitPaymentPageLoad: return "";
			case LogCodes.FitResultsPageLoad: return "";
			case LogCodes.FpApiAuthResp: return "";
			case LogCodes.FpApiClosePopup: return "";
			case LogCodes.FpApiIframeErrors: return "";
			case LogCodes.FpApiLoadIframe: return "";
			case LogCodes.FpApiPreSubmit2: return "";
			case LogCodes.FpApiPreSubmitFail: return "";
			case LogCodes.FpApiPreSubmitUniqueId: return "";
			case LogCodes.FpApiSoftDecline: return "";
			case LogCodes.FpApiSubmitReq: return "";
			case LogCodes.FpEmailNotSupported: return "Freedom Pay Email not supported.";
			case LogCodes.Ga4TransActivities: return "";
			case LogCodes.Ga4TransBedroom: return "";
			case LogCodes.Ga4TransFit: return "";
			case LogCodes.HandlerDetails: return "";
			case LogCodes.HandleResults: return "";
			case LogCodes.Http200: return "HTTP API Status 200 - OK";
			case LogCodes.Http500: return "HTTP API Status 500 - Internal Server Error";
			case LogCodes.IFrameError: return "";
			case LogCodes.IocReady: return "The IoC dependency injection process has completed on startup.";
			case LogCodes.IpAddress: return "";
			case LogCodes.ItIssAdd: return "";
			case LogCodes.ItProjAdd: return "";
			case LogCodes.LoadCountries: return "Loading Country Codes.";
			case LogCodes.LoadDiscountCodes: return "Loading discount codes from XML.";
			case LogCodes.LoadEmailsJson: return "Loading Emails XML";
			case LogCodes.LoadEmailsXmlConfig: return "Failed to load Emails.Config in DxEmails";
			case LogCodes.LoadHtmlConfig: return "";
			case LogCodes.LoadOwsErrors: return "";
			case LogCodes.LoadRoomTypeFilters: return "";
			case LogCodes.LoadRoomTypes: return "";
			case LogCodes.LoadTerms: return "";
			case LogCodes.LoadTitles: return "";
			case LogCodes.LogCart: return "Logging cart data.";
			case LogCodes.LogError: return "An error has occurred while trying to log error or information in any of the log operations.";
			case LogCodes.LowPrice: return "A low price has been detected.";
			case LogCodes.MasterController: return "WBS API POST call to HandleMaster.";
			case LogCodes.OwsAddComments: return "Adding comments to an Opera reservation.";
			case LogCodes.OwsAddPayment: return "Adding a payment record to Opera after a successful payment.";
			case LogCodes.OwsAvailGen: return "Opera Requesting general availability. OperaCloudAvailabilityService.RequestAvailability.";
			case LogCodes.OwsCriticalError: return "";
			case LogCodes.OwsError: return "";
			case LogCodes.OwsFetchBooking: return "Opera Fetch Booking.";
			case LogCodes.OwsLibEx: return "An error has occurred while throwing a new LibException.";
			case LogCodes.OwsMakeBooking: return "Opera Make Booking. OperaCloudReservationService.MakeBooking(Async).";
			case LogCodes.OwsMakeBookingCritical: return "Opera Make Booking Critical Error. OperaCloudReservationService.MakeBooking(Async).";
			case LogCodes.OwsModifyBooking: return "Opera Modify Booking - after successful payment.";
			case LogCodes.OwsOperaParseFault: return "";
			case LogCodes.OwsPartyComments: return "Opera adding comment to link parties for a multiple room booking.";
			case LogCodes.OwsSchemeMap: return "Map for a scheme not found in OperaCloudConfig.xml";
			case LogCodes.OwsUpdatePackage: return "Opera update package.";
			case LogCodes.OwsZeroPrice: return "A zero price value has been returned by Opera. ProcessGeneralAvailResponseSegment.";
			case LogCodes.Paid: return "A PAID Reason Code 100 received from FreedomPay";
			case LogCodes.PayFpBadState: return "The payment state is out of sequence. This should be investigated.";
			case LogCodes.PayFpGetIframe: return "Requesting payment iframe (overlay) from FreedomPay.";
			case LogCodes.PayFpGotIframe: return "Got FreedomPay iframe (overlay) and returning to the client (javascript).";
			case LogCodes.PayFpLocked: return "The payment process has been locked. The payment state cannot be determined.";
			case LogCodes.PayFpPaidAck: return "A payment has been accepted and an ACK received from the gateway.";
			case LogCodes.PayFpRestart: return "The payment process is being restarted. After any of: user refreshed payment page; user closed payment overlay; card/attempt rejection reloaded page.";
			case LogCodes.PayFpSubmit: return "iframe requests a submit. WBS submits payment to FreedomPay.";
			case LogCodes.PayFpSubmitAccepted: return "The payment submit has been accepted.";
			case LogCodes.PayFpSubmitAcceptSendAck: return "Send the ACK to FreedomPay";
			case LogCodes.PayFpSubmitNotAccepted: return "FreedomPay submit was not accepted.";
			case LogCodes.PayFpTestGetIframe: return "";
			case LogCodes.PaymentPageLoad: return "";
			case LogCodes.PayPaidAuth: return "";
			case LogCodes.PayPreBook: return "";
			case LogCodes.PayResults: return "";
			case LogCodes.Pc500Error: return "";
			case LogCodes.PcAcceptTerms: return "";
			case LogCodes.PcAddCartItem: return "";
			case LogCodes.PcApiSendEmail: return "";
			case LogCodes.PcAppointmentType: return "PcBaseService.PopulateTab when loading TabItems in Admin.";
			case LogCodes.PcBill: return "WBS API call to post a PC Billing record.";
			case LogCodes.PcBook: return "";
			case LogCodes.PcBookAppointment: return "";
			case LogCodes.PcBookingSupport: return "Loading the Booking Support page, or sending Booking Support emails.";
			case LogCodes.PcCartClear: return "";
			case LogCodes.PcCartClearAddItem: return "";
			case LogCodes.PcCartDelItem: return "";
			case LogCodes.PcCartList: return "";
			case LogCodes.PcClients: return "";
			case LogCodes.PcConfigDates: return "";
			case LogCodes.PcConfirmation: return "WBS API call to get confirmation cshtml.";
			case LogCodes.PcConfirmationPageLoad: return "";
			case LogCodes.PcCreateBookingPrePay: return "WBS API call to create booking, pre-payment. The user has clicked Make Secure Payment.";
			case LogCodes.PcCreateClient: return "";
			case LogCodes.PcCriticalError: return "";
			case LogCodes.PcCrossSell: return "WBS API POST UpSellItems. WBS is looking for cross/up-sell items to display on a BOOK NOW view.";
			case LogCodes.PcDepartments: return "Get Activities Departments.";
			case LogCodes.PcEmailConf: return "PC Email Confirmation.";
			case LogCodes.PcFeedback: return "Sending Feedback emails.";
			case LogCodes.PcGetReservation: return "";
			case LogCodes.PcGlobal: return "Get Activities Global Data.";
			case LogCodes.PcPackageSearch: return "";
			case LogCodes.PcPackSlots: return "";
			case LogCodes.PcPaid: return "Following a submit payment (user has clicked PAY on overlay), the FP gateway has returned ACCEPT. Note that ReasonCode and failed ACK may still occur.";
			case LogCodes.PcPatchReservation: return "";
			case LogCodes.PcPayApiLoad: return "WBS API call to load payment page data, after Payment page load.";
			case LogCodes.PcPayCheckResult: return "WBS API call to check that there is a successful Order ID (reservation).";
			case LogCodes.PcPayGetIframe: return "WBS API call to get the FP payment iframe. The user has clicked Make Secure Payment, and a booking has been made, so start prepare the payment gateway overlay.";
			case LogCodes.PcPayment: return "WBS API call to load payment page data, after Payment page load.";
			case LogCodes.PcPaymentPageInit: return "";
			case LogCodes.PcPaymentPageLoad: return "";
			case LogCodes.PcPayUpdateContact: return "WBS API call to update contact details (lead client).";
			case LogCodes.PcPayUpdateSummary: return "WBS API call to update booking summary.";
			case LogCodes.PcPayUpdateViewData: return "WBS API call to update payment details.";
			case LogCodes.PcQuick: return "WBS API GET QuickAvailability. Get the availability of tab items on a tab page. Used to show availability, price, 'Per Person' etc.";
			case LogCodes.PcQuickSearchRooms: return "";
			case LogCodes.PcRemoveCartItem: return "";
			case LogCodes.PcReport: return "";
			case LogCodes.PcResClear: return "";
			case LogCodes.PcResetPayment: return "";
			case LogCodes.PcRulesStatus: return "";
			case LogCodes.PcSaveRes: return "Register a successful PC reservation, post billing, ready for reporting.";
			case LogCodes.PcSearchComp: return "WBS API POST SearchPackageComponents. The user has selected a package, and WBS is checking availability of the package components.";
			case LogCodes.PcSearchContent: return "Loading DepartmentVmAsync, SearchContent.cshtml";
			case LogCodes.PcSearchItem: return "WBS API POST Availability. The user has clicked BOOK NOW on a tab item card.";
			case LogCodes.PcSearchPageLoad: return "";
			case LogCodes.PcSendEmail: return "WBS API call to get confirmation data and send customer/elh emails, if not already sent. Or SendEmailAsync.SendEmailAsync.";
			case LogCodes.PcSendFeedback: return "";
			case LogCodes.PcServiceCharge: return "";
			case LogCodes.PcServiceChargeError: return "";
			case LogCodes.PcSessionNew: return "";
			case LogCodes.PcSummaryPageLoad: return "";
			case LogCodes.PcTestPaymentOverlay: return "Testing the payment overlay.";
			case LogCodes.PlanCheck: return "";
			case LogCodes.PlanDuplicate: return "";
			case LogCodes.PreSearchQuery: return "";
			case LogCodes.PreSearchUri: return "";
			case LogCodes.ProfileUpdate: return "";
			case LogCodes.QsTest: return "Query String Test.";
			case LogCodes.RatePlanCode: return "";
			case LogCodes.Redirector: return "";
			case LogCodes.ResultMissing: return "";
			case LogCodes.ResultsPageLoad: return "";
			case LogCodes.ResultsPopDisc: return "Populating results discounted (old) rate plans.";
			case LogCodes.ResultsPopSingle: return "Populating results single (offer) rate plans.";
			case LogCodes.ResultsRedirect: return "";
			case LogCodes.ResultsUrl: return "";
			case LogCodes.Search: return "";
			case LogCodes.SearchDates: return "";
			case LogCodes.SearchMini: return "";
			case LogCodes.SearchPageLoad: return "Loading the Search Page";
			case LogCodes.SearchSummary: return "";
			case LogCodes.SearchUpdate: return "";
			case LogCodes.SendEmail: return "Sending an email - BaseEmailService.";
			case LogCodes.SendEmailLp: return "Failed to send an email on detecting a low price.";
			case LogCodes.ServerTime: return "WBS API GET to get the server time (as opposed to the client time).";
			case LogCodes.SessionTimeout: return "";
			case LogCodes.Snippet: return "";
			case LogCodes.SnippetMissing: return "";
			case LogCodes.SnippetVars: return "";
			case LogCodes.SpecialsNone: return "";
			case LogCodes.StringProperCase: return "";
			case LogCodes.SysBackgroundJob: return "A background job that performs operations outside any user context.";
			case LogCodes.SysBackgroundJobAction: return "Performing a background job action.";
			case LogCodes.SysReloadCache: return "";
			case LogCodes.SysSessionEnd: return "";
			case LogCodes.SysSessionStart: return "";
			case LogCodes.SysTrace: return "A sequence of trace messages.";
			case LogCodes.TelephoneMap: return "";
			case LogCodes.TestError: return "";
			case LogCodes.TestInfo: return "";
			case LogCodes.UpdateBooking: return "";
			case LogCodes.UserAgent: return "";
			case LogCodes.ValidateBooking: return "Is the reservation (reservationId) valid for a one-time discount?";
			case LogCodes.VouchersAdd: return "";
			case LogCodes.VouchersCheck: return "";
			case LogCodes.VouchersClose: return "";
			case LogCodes.VouchersFind: return "";
			case LogCodes.VouchersLimitDeposit: return "";
			case LogCodes.VouchersLoad: return "";
			case LogCodes.VouchersRedeem: return "";
			case LogCodes.VouchersReverse: return "";
			case LogCodes.XmlLog: return "An error has occurred while logging XML in LogXmlService.LogXmlText.";
			default: return string.Empty;
		}

	}
}