using System.Text;
using System.Xml;
using System.Xml.Linq;
using Msh.Common.Constants;
using Msh.Common.Logger;
using Msh.Common.Models.OwsCommon;
using Msh.Common.Services;
using Msh.Opera.Ows.ExtensionMethods;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.AvailabilityResponses;
using Msh.Opera.Ows.Models.ReservationRequestModels;
using Msh.Opera.Ows.Models.ReservationResponseModels;
using Msh.Opera.Ows.Services.Base;
using Msh.Opera.Ows.Services.Builders;
using Msh.Opera.Ows.Services.Config;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Calls OWS cloud services for reservations
/// </summary>
public class OperaReservationService(
	IOwsConfigService owsConfigService,
	IOwsPostService owsPostService,
	IReservationBuildService reservationBuildService,
	ILogXmlService logXmlService)
	: OperaBaseService(owsConfigService.OwsConfig, logXmlService, owsConfigService, owsPostService),
		IOperaReservationService
{


	public string LastRequest { get; private set; } = string.Empty;

	public async Task<(OwsReservation owsReservation, OwsResult owsResult)> CreateBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor)
	{
		var config = _config;
		reqData.Modify = false;
		return await CreateModifyBookingAsync(reqData, redactor, config);
	}

	public async Task<(OwsReservation owsReservation, OwsResult owsResult)> ModifyBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor)
	{
		var config = _config;
		reqData.Modify = true;
		return await CreateModifyBookingAsync(reqData, redactor, config);
	}

	public async Task<(OwsReservation owsReservation, OwsResult owsResult)> FetchBookingAsync(OwsBaseSession reqData, string reservationId)
	{
		var config = _config;

		const string mainElement = "FetchBookingResponse";

		var xElement = reservationBuildService.FetchBooking(reqData, reservationId, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.FormatXml(Formatting.Indented).ToString();

		var (xdoc, contents, owsResult) = await PostAsync(sb.FormatXml(Formatting.None), config.ReservationUrl());

		var decode = DecodeOwsReservation(xdoc, contents, mainElement);

		return (decode.owsReservation, decode.owsResult ?? owsResult);
	}
	public async Task<(OwsPackageExtraRes owsPackageExtraRes, OwsResult owsResult)> UpdatePackageAsync(OwsPackageRequest reqData)
	{
		var config = _config;

		var xElement = reservationBuildService.UpdatePackages(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.FormatXml(Formatting.Indented).ToString();

		await _logXmlService.LogXmlText(LastRequest, "UpdatePackageReq");

		var (xdoc, contents, owsResult) = await PostAsync(sb.FormatXml(Formatting.None), config.ReservationUrl());

		await _logXmlService.LogXmlText(contents, "UpdatePackageRes");

		var decode = DecodeOwsPackage(xdoc, contents);

		return (decode.owsPackageExtraRes, decode.owsResult ?? owsResult);
	}

	public async Task<(CommentList list, OwsResult owsResult)> AddBookingCommentsAsync(OwsAddBookingCommentRequest reqData)
	{
		var config = _config;

		var xElement = reservationBuildService.AddBookingComments(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.FormatXml(Formatting.Indented).ToString();

		await _logXmlService.LogXmlText(LastRequest, "AddBookingCommentsReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb.FormatXml(Formatting.None), config.ReservationUrl(), reqData.SessionKey);

		await _logXmlService.LogXmlText(contents, "AddBookingCommentsRes", reqData.SessionKey);

		var decode = DecodeOwsAddedComments(xdoc, contents);

		return (decode.list, decode.owsResult ?? owsResult);
	}

	public async Task<(string resvId, OwsResult owsResult)> AddBookingPaymentAsync(OwsAddPaymentRequest reqData)
	{
		var config = _config;

		var xElement = reservationBuildService.AddPayment(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.FormatXml(Formatting.Indented).ToString();

		await _logXmlService.LogXmlText(LastRequest, "AddBookingPaymentReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb.FormatXml(Formatting.None), config.ResvAdvancedUrl(), reqData.SessionKey);

		await _logXmlService.LogXmlText(contents, "AddBookingPaymentRes", reqData.SessionKey);

		var decode = DecodePayment(xdoc, contents);

		return (decode.resvId, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsReservation owsReservation, OwsResult owsResult)> GetReservationStatusAsync(OwsBaseSession reqData, string hotelCode, string reservationId, IXmlRedactor redactor, OwsConfig config)
	{
		var xElement = reservationBuildService.GetReservationStatus(reqData, hotelCode, reservationId, config);

		var sb = new StringBuilder(xElement.ToString());


		LastRequest = sb.FormatXml(Formatting.Indented).ToString();

		await _logXmlService.LogXml(xElement, "BookStatusReq", reqData.SessionKey, redactor);
		//_logXmlService.LogXmlText(LastRequest, keyReq, reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb.FormatXml(Formatting.None), config.ReservationUrl(), reqData.SessionKey);


		await _logXmlService.LogXml(contents, "BookStatusRes", reqData.SessionKey, redactor);

		var mainElement = "GetReservationStatusResponse";
		var decode = DecodeOwsReservationStatus(xdoc, contents, mainElement);

		return (decode.owsReservation, decode.owsResult ?? owsResult);
	}

	private (string resvId, OwsResult owsResult) DecodePayment(XDocument xdocInput, string contents)
	{
		const string methodName = "DecodePayment";
		const string mainElement = "MakePaymentResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (string.Empty, owsResult);

		return (string.Empty, new OwsResult { ResultStatusFlag = CommonConst.OwsResultStatusFlag.Success });
	}

	private (CommentList list, OwsResult owsResult) DecodeOwsAddedComments(XDocument xdocInput, string contents)
	{
		const string methodName = "DecodeOwsAddedComments";
		const string mainElement = "GuestRequestsResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as CommentList, owsResult);

		var data = xdoc.Descendants("GuestRequestsResponse").Descendants("Comments")
			.Select(p => new
			{
				OwsComment = p.Descendants("Comment")
					.Select(pi => new OwsComment
					{
						GuestViewable = pi.ValueA(false, "guestViewable"),
						Text = pi.ValueE("Text"),
						CommentId = pi.ValueE("CommentId")
					}).ToList(),


			}).SingleOrDefault();

		var list = new CommentList();
		list.Comments.AddRange(data?.OwsComment ?? new List<OwsComment>());

		return (list, new OwsResult { ResultStatusFlag = CommonConst.OwsResultStatusFlag.Success });
	}


	protected async Task<(OwsReservation owsReservation, OwsResult owsResult)> CreateModifyBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor, OwsConfig config)
	{
		var xElement = reservationBuildService.MakeReservation(reqData, config);

		var sb = new StringBuilder(xElement.ToString());


		LastRequest = sb.FormatXml(Formatting.Indented).ToString();

		await _logXmlService.LogXml(xElement, reqData.Modify ? "BookModReq" : "BookReq", reqData.SessionKey, redactor);

		var (xdoc, contents, owsResult) = await PostAsync(sb.FormatXml(Formatting.None), config.ReservationUrl(), reqData.SessionKey);

		await _logXmlService.LogXml(contents, reqData.Modify ? "BookModRes" : "BookRes", reqData.SessionKey, redactor);

		var mainElement = reqData.Modify ? "ModifyBookingResponse" : "CreateBookingResponse";

		var decode = DecodeOwsReservation(xdoc, contents, mainElement);

		return (decode.owsReservation, decode.owsResult ?? owsResult);
	}


	/// <summary>
	/// WORK IN PROGRESS - THIS DOES NOT IMPLEMENT THE DECODING OF RESERVATION STATUS, YET
	/// </summary>
	/// <param name="xdocInput"></param>
	/// <param name="contents"></param>
	/// <param name="mainElement"></param>
	/// <returns></returns>
	protected (OwsReservation owsReservation, OwsResult owsResult) DecodeOwsReservationStatus(XDocument xdocInput, string contents, string mainElement)
	{
		const string methodName = "DecodeOwsReservationStatus";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsReservation, owsResult);

		var owsReservation = xdoc.Descendants("HotelReservation")
			.Select(res => new OwsReservation
			{
				ReservationStatus = res.ValueA("reservationStatus").Get<BookingStatus>(),

				OwsUniqueIds = res.Descendants("UniqueIDList").Descendants("UniqueID")
					.Select(uid => GetOwsUniqueId(uid)).ToList(),

				RoomStay = res.Descendants("RoomStay")
					.Select(stay => new OwsRoomStayRes
					{
						RatePlans = stay.Descendants("RatePlans").Descendants("RatePlan")
							.Select(rp => new OwsRatePlanRes
							{
								RatePlanCode = rp.ValueA("ratePlanCode"),
								SuppressRate = rp.ValueA(false, "suppressRate"),
								RatePlanName = rp.ValueA("ratePlanName"),
								Description = rp.Descendant("RatePlanDescription", "Text").ValueE()
							}).ToList(), // .FirstOrDefault()
						// Todo - OwsRoomStayRes - Under what circumstances would OWS ever return multiple?

						RoomType = stay.Descendants("RoomTypes").Descendants("RoomType")
							.Select(rt => new OwsRoomTypeRes
							{
								RoomTypeCode = rt.ValueA("roomTypeCode"),
								NumberOfUnits = rt.ValueA(0, "numberOfUnits"),
								Description = rt.Descendant("RatePlanDescription", "Text").ValueE(),
								ShortDescription = rt.Descendant("RoomTypeShortDescription", "Text").ValueE()

							}).FirstOrDefault(),

						// This is a list because it breaks down into daily rates
						RoomRates = stay.Descendants("RoomRates").Descendants("RoomRate")
							.Select(rr => new OwsRoomRateRes
							{
								RoomTypeCode = rr.ValueA("roomTypeCode"),
								RatePlanCode = rr.ValueA("ratePlanCode"),
								EffectiveDate = rr.Descendant("Rates", "Rate").ValueA(DateTime.MinValue, "effectiveDate"),
								Rate = rr.Descendant("Rates", "Rate").GetSoapAmount("Base")
							}).ToList(),

						GuestCountsIsPerRoom = stay.Descendant("GuestCounts").ValueA(false, "isPerRoom"),
						GuestCounts = stay.Descendants("GuestCounts").Descendants("GuestCount")
							.Select(gc => new OwsGuestCount
							{
								AgeQualifyingCode = gc.ValueA("ageQualifyingCode").Get<OwsAgeQualifyingCode>(),
								Count = gc.ValueA(0, "count")
							}).ToList(),
						Arrive = stay.Descendant("TimeSpan", "StartDate").ValueE(DateTime.MinValue),
						Depart = stay.Descendant("TimeSpan", "EndDate").ValueE(DateTime.MinValue),
						HotelCode = stay.Descendant("HotelReference").ValueA("hotelCode"),
						ChainCode = stay.Descendant("HotelReference").ValueA("chainCode"),
						Total = stay.GetSoapAmount("Total"),

						Guarantee = stay.Descendants("Guarantee")
							.Select(g => new OwsGuarantee
							{
								GuaranteeType = g.ValueA("guaranteeType"),
								Description = g.Descendant("GuaranteeDescription", "Text").ValueE()
							}).SingleOrDefault(),

						PaymentsAccepted = stay.Descendants("Payment").Descendants("PaymentsAccepted")
							.Select(g => new OwsPaymentAccepted
							{
								PaymentType = g.Descendant("PaymentType", "OtherPayment").ValueA("type")
							}).ToList(),

						Comments = stay.Descendants("Comments").Descendants("Comment")
							.Select(c => new OwsComment
							{
								Text = c.Descendant("Text").ValueE(),
								GuestViewable = c.ValueA(false, "guestViewable")
							}).ToList(),

						Packages = stay.Descendants("Packages").Descendants("Package")
							.Select(p => new OwsPackageRes
							{
								PackageCode = p.ValueA("packageCode"),
								Source = p.ValueA("source"),
								TaxIncluded = p.ValueA(false, "taxIncluded"),
								PackageAmount = p.GetSoapAmount("PackageAmount"),
								TaxAmount = p.GetSoapAmount("TaxAmount"),
								Allowance = p.GetSoapAmount("Allowance")
							}).ToList(),

						ExpectedCharges = stay.Descendants("ExpectedCharges")
							.Select(ecs => new OwsExpectedCharges
							{
								TotalRoomRateAndPackages = ecs.GetSoapAmountNamedAttribute("TotalRoomRateAndPackages"),
								TotalTaxesAndFees = ecs.GetSoapAmountNamedAttribute("TotalTaxesAndFees"),
								TaxInclusive = ecs.ValueA(false, "TaxInclusive"),
								DayCharges = ecs.Descendants("ChargesForPostingDate")
									.Select(dcs => new OwsPostingDateCharge
									{
										PostingDate = dcs.ValueA(DateTime.MinValue, "PostingDate"),
										RoomRateAndPackages = GetExtendedCharges(dcs, OperaChargeTypes.RoomRateAndPackages),
										TaxesAndFees = GetExtendedCharges(dcs, OperaChargeTypes.TaxesAndFees),
									}).ToList()

							}).FirstOrDefault()

					}).SingleOrDefault(),

				Guests = res.Descendants("ResGuests").Descendants("ResGuest").Descendants("Profiles").Descendants("Profile")
					.Select(g => new OwsReservationGuest
					{
						ProfileId = GetGuestOwsUniqueId(g.Descendant("ProfileIDs")),
						Title = g.Descendant("Customer", "PersonName").ValueE("nameTitle"),
						FirstName = g.Descendant("Customer", "PersonName").ValueE("firstName"),
						LastName = g.Descendant("Customer", "PersonName").ValueE("lastName"),
						Address = g.Descendants("Addresses").Descendants("NameAddress")
							.Select(a => new OwsReservationAddress
							{
								AddressLine1 = a.Descendants("AddressLine").FirstOrDefault().ValueE(),
								AddressLine2 = a.Descendants("AddressLine").Skip(1).FirstOrDefault().ValueE(),
								City = a.ValueE("cityName"),
								CountryCode = a.ValueE("countryCode"),
								PostalCode = a.ValueE("postalCode"),
							}).SingleOrDefault(),

						Phones = g.Descendants("Phones").Descendants("NamePhone")
							.Select(p => new OwsPhone
							{
								PhoneType = p.ValueA("phoneType"),
								PhoneRole = p.ValueA("phoneRole"),
								Primary = p.ValueA(false, "primary"),
								Telephone = p.Descendant("PhoneNumber").ValueE()
							}).ToList(),

						Emails = g.Descendants("EMails").Descendants("NameEmail")
							.Select(e => new OwsEmail
							{
								Primary = e.ValueA(false, "primary"),
								EmailType = e.ValueA("emailType"),
								Email = e.ValueE()
							}).ToList()
					}).ToList(),

				ReservationHistory = res.Descendants("ReservationHistory")
					.Select(rh => new OwsReservationHistory
					{
						InsertUser = rh.ValueA("insertUser"),
						InsertDate = rh.ValueA(DateTime.MinValue, "insertDate"),
						UpdateUser = rh.ValueA("updateUser"),
						UpdateDate = rh.ValueA(DateTime.MinValue, "updateDate"),
					}).FirstOrDefault()


			}).SingleOrDefault();

		return (owsReservation, new OwsResult { ResultStatusFlag = CommonConst.OwsResultStatusFlag.Success });
	}


	protected (OwsReservation owsReservation, OwsResult owsResult) DecodeOwsReservation(XDocument xdocInput, string contents, string mainElement)
	{
		const string methodName = "DecodeOwsReservation";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsReservation, owsResult);

		var owsReservation = xdoc.Descendants("HotelReservation")
			.Select(res => new OwsReservation
			{
				ReservationStatus = res.ValueA("reservationStatus").Get<BookingStatus>(),

				OwsUniqueIds = res.Descendants("UniqueIDList").Descendants("UniqueID")
					.Select(uid => GetOwsUniqueId(uid)).ToList(),

				RoomStay = res.Descendants("RoomStay")
					.Select(stay => new OwsRoomStayRes
					{
						RatePlans = stay.Descendants("RatePlans").Descendants("RatePlan")
							.Select(rp => new OwsRatePlanRes
							{
								RatePlanCode = rp.ValueA("ratePlanCode"),
								SuppressRate = rp.ValueA(false, "suppressRate"),
								RatePlanName = rp.ValueA("ratePlanName"),
								Description = rp.Descendant("RatePlanDescription", "Text").ValueE()
							}).ToList(), // .FirstOrDefault()
						// Todo - OwsRoomStayRes - Under what circumstances would OWS ever return multiple?

						RoomType = stay.Descendants("RoomTypes").Descendants("RoomType")
							.Select(rt => new OwsRoomTypeRes
							{
								RoomTypeCode = rt.ValueA("roomTypeCode"),
								NumberOfUnits = rt.ValueA(0, "numberOfUnits"),
								Description = rt.Descendant("RatePlanDescription", "Text").ValueE(),
								ShortDescription = rt.Descendant("RoomTypeShortDescription", "Text").ValueE()

							}).FirstOrDefault(),

						// This is a list because it breaks down into daily rates
						RoomRates = stay.Descendants("RoomRates").Descendants("RoomRate")
							.Select(rr => new OwsRoomRateRes
							{
								RoomTypeCode = rr.ValueA("roomTypeCode"),
								RatePlanCode = rr.ValueA("ratePlanCode"),
								EffectiveDate = rr.Descendant("Rates", "Rate").ValueA(DateTime.MinValue, "effectiveDate"),
								Rate = rr.Descendant("Rates", "Rate").GetSoapAmount("Base")
							}).ToList(),

						GuestCountsIsPerRoom = stay.Descendant("GuestCounts").ValueA(false, "isPerRoom"),
						GuestCounts = stay.Descendants("GuestCounts").Descendants("GuestCount")
							.Select(gc => new OwsGuestCount
							{
								AgeQualifyingCode = gc.ValueA("ageQualifyingCode").Get<OwsAgeQualifyingCode>(),
								Count = gc.ValueA(0, "count")
							}).ToList(),
						Arrive = stay.Descendant("TimeSpan", "StartDate").ValueE(DateTime.MinValue),
						Depart = stay.Descendant("TimeSpan", "EndDate").ValueE(DateTime.MinValue),
						HotelCode = stay.Descendant("HotelReference").ValueA("hotelCode"),
						ChainCode = stay.Descendant("HotelReference").ValueA("chainCode"),
						Total = stay.GetSoapAmount("Total"),

						Guarantee = stay.Descendants("Guarantee")
							.Select(g => new OwsGuarantee
							{
								GuaranteeType = g.ValueA("guaranteeType"),
								Description = g.Descendant("GuaranteeDescription", "Text").ValueE()
							}).SingleOrDefault(),

						PaymentsAccepted = stay.Descendants("Payment").Descendants("PaymentsAccepted")
							.Select(g => new OwsPaymentAccepted
							{
								PaymentType = g.Descendant("PaymentType", "OtherPayment").ValueA("type")
							}).ToList(),

						Comments = stay.Descendants("Comments").Descendants("Comment")
							.Select(c => new OwsComment
							{
								Text = c.Descendant("Text").ValueE(),
								GuestViewable = c.ValueA(false, "guestViewable")
							}).ToList(),

						Packages = stay.Descendants("Packages").Descendants("Package")
							.Select(p => new OwsPackageRes
							{
								PackageCode = p.ValueA("packageCode"),
								Source = p.ValueA("source"),
								TaxIncluded = p.ValueA(false, "taxIncluded"),
								PackageAmount = p.GetSoapAmount("PackageAmount"),
								TaxAmount = p.GetSoapAmount("TaxAmount"),
								Allowance = p.GetSoapAmount("Allowance")
							}).ToList(),

						ExpectedCharges = stay.Descendants("ExpectedCharges")
							.Select(ecs => new OwsExpectedCharges
							{
								TotalRoomRateAndPackages = ecs.GetSoapAmountNamedAttribute("TotalRoomRateAndPackages"),
								TotalTaxesAndFees = ecs.GetSoapAmountNamedAttribute("TotalTaxesAndFees"),
								TaxInclusive = ecs.ValueA(false, "TaxInclusive"),
								DayCharges = ecs.Descendants("ChargesForPostingDate")
									.Select(dcs => new OwsPostingDateCharge
									{
										PostingDate = dcs.ValueA(DateTime.MinValue, "PostingDate"),
										RoomRateAndPackages = GetExtendedCharges(dcs, OperaChargeTypes.RoomRateAndPackages),
										TaxesAndFees = GetExtendedCharges(dcs, OperaChargeTypes.TaxesAndFees),
									}).ToList()

							}).FirstOrDefault()

					}).SingleOrDefault(),

				Guests = res.Descendants("ResGuests").Descendants("ResGuest").Descendants("Profiles").Descendants("Profile")
					.Select(g => new OwsReservationGuest
					{
						ProfileId = GetGuestOwsUniqueId(g.Descendant("ProfileIDs")),
						Title = g.Descendant("Customer", "PersonName").ValueE("nameTitle"),
						FirstName = g.Descendant("Customer", "PersonName").ValueE("firstName"),
						LastName = g.Descendant("Customer", "PersonName").ValueE("lastName"),
						Address = g.Descendants("Addresses").Descendants("NameAddress")
							.Select(a => new OwsReservationAddress
							{
								AddressLine1 = a.Descendants("AddressLine").FirstOrDefault().ValueE(),
								AddressLine2 = a.Descendants("AddressLine").Skip(1).FirstOrDefault().ValueE(),
								City = a.ValueE("cityName"),
								CountryCode = a.ValueE("countryCode"),
								PostalCode = a.ValueE("postalCode"),
							}).SingleOrDefault(),

						Phones = g.Descendants("Phones").Descendants("NamePhone")
							.Select(p => new OwsPhone
							{
								PhoneType = p.ValueA("phoneType"),
								PhoneRole = p.ValueA("phoneRole"),
								Primary = p.ValueA(false, "primary"),
								Telephone = p.Descendant("PhoneNumber").ValueE()
							}).ToList(),

						Emails = g.Descendants("EMails").Descendants("NameEmail")
							.Select(e => new OwsEmail
							{
								Primary = e.ValueA(false, "primary"),
								EmailType = e.ValueA("emailType"),
								Email = e.ValueE()
							}).ToList()
					}).ToList(),

				ReservationHistory = res.Descendants("ReservationHistory")
					.Select(rh => new OwsReservationHistory
					{
						InsertUser = rh.ValueA("insertUser"),
						InsertDate = rh.ValueA(DateTime.MinValue, "insertDate"),
						UpdateUser = rh.ValueA("updateUser"),
						UpdateDate = rh.ValueA(DateTime.MinValue, "updateDate"),
					}).FirstOrDefault()


			}).SingleOrDefault();

		return (owsReservation, new OwsResult { ResultStatusFlag = CommonConst.OwsResultStatusFlag.Success });
	}

	protected (OwsPackageExtraRes owsPackageExtraRes, OwsResult owsResult) DecodeOwsPackage(XDocument xdocInput, string contents)
	{
		const string methodName = "DecodeOwsPackage";
		const string mainElement = "UpdatePackageResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsPackageExtraRes, owsResult);

		var package = xdoc.Descendants("BookedPackageList").Descendants("PackageDetails")
			.Select(p => new OwsPackageExtraRes
			{
				PackageInfo = p.Descendants("PackageInfo")
					.Select(pi => new OwsPackageInfo
					{
						PackageCode = pi.ValueA("packageCode"),
						CalculationRule = pi.ValueA("calculationRule"),
						PostingRhythm = pi.ValueA("postingRhythm"),
						TaxIncluded = pi.ValueA(false, "taxIncluded"),
						Amount = pi.GetSoapAmount("Amount"),
						Description = pi.Descendant("Description", "Text", "textElement").ValueE(),
						ShortDescription = pi.Descendant("ShortDescription", "Text", "textElement").ValueE()

					}).FirstOrDefault(),

				ExpectedCharges = p.Descendants("PackageInfo").Descendants("PackageCharge")
					.Select(pc => new OwsPackageCharge
					{
						PackageCode = pc.ValueE("PackageCode"),
						StartDate = pc.Descendant("ValidDates").ValueE(DateTime.MinValue, "StartDate"),
						EndDate = pc.Descendant("ValidDates").ValueE(DateTime.MinValue, "EndDate"),
						UnitAmount = pc.GetSoapAmount("UnitAmount"),
						Quantity = pc.ValueE(0, "Quantity"),
						Tax = pc.GetSoapAmount("Tax"),
						Total = pc.GetSoapAmount("TotalAmount")
					}).SingleOrDefault()

			}).FirstOrDefault();


		return (package, new OwsResult { ResultStatusFlag = CommonConst.OwsResultStatusFlag.Success });
	}

}