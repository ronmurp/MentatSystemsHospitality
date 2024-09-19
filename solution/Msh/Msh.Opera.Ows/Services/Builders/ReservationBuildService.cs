using System.Xml.Linq;
using Msh.Common.Models;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.ReservationRequestModels;
using Msh.Opera.Ows.Models.ReservationResponseModels;

namespace Msh.Opera.Ows.Services.Builders;

public class ReservationBuildService : IReservationBuildService
{
	private readonly ISoapEnvelopeService _soapEnvelopeService;
	private readonly XNamespace res = "http://webservices.micros.com/ows/5.1/Reservation.wsdl";
	private readonly XNamespace res1 = "http://webservices.micros.com/og/4.3/Reservation/";
	private readonly XNamespace hot = "http://webservices.micros.com/og/4.3/HotelCommon/";
	private readonly XNamespace name = "http://webservices.micros.com/og/4.3/Name/";
	private readonly XNamespace com = "http://webservices.micros.com/og/4.3/Common/";

	private readonly XNamespace resAdv = "http://webservices.micros.com/og/4.3/ResvAdvanced/";
	private readonly XNamespace resAdvWsdl = "http://webservices.micros.com/og/4.3/ResvAdvanced.wsdl";
	public ReservationBuildService(ISoapEnvelopeService soapEnvelopeService)
	{
		_soapEnvelopeService = soapEnvelopeService;
	}

	public XElement GetReservationStatus(OwsBaseSession reqData, string hotelCode, string reservationId, OwsConfig config)
	{
		var cr = new XElement(res + "GetReservationStatusRequest",
			new XAttribute(XNamespace.Xmlns + "res", res),
			new XAttribute(XNamespace.Xmlns + "res1", res1),
			new XAttribute(XNamespace.Xmlns + "hot", hot),
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "com", com),
			//new XElement(res + "HotelReference",
			//    new XAttribute("chainCode", config.ChainCode),
			//    new XAttribute("hotelCode", hotelCode)),
			new XElement(res + "ConfirmationNumber", reservationId)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, cr, OwsService.Reservation, config);

		return env;
	}
	public XElement MakeReservation(OwsReservationRequest reqData, OwsConfig config)
	{
		var request = reqData.Modify ? "ModifyBookingRequest" : "CreateBookingRequest";

		var xElementGuests = GetResGuests(reqData);

		var cr = new XElement(res + request,
			new XAttribute(XNamespace.Xmlns + "res", res),
			new XAttribute(XNamespace.Xmlns + "res1", res1),
			new XAttribute(XNamespace.Xmlns + "hot", hot),
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "com", com),
			new XElement(res + "HotelReservation",
				//new XAttribute("reservationAction", $"{reqData.ReservationAction}"),
				new XAttribute("marketSegment", ""), // Todo - FIT - Reservation - marketSegment?
				new XAttribute("sourceCode", "SPW"), // Todo - FIT - Reservation - SourceCode?
				GetUniqueIdList(reqData),
				new XElement(res1 + "RoomStays",
					new XElement(hot + "RoomStay",
						new XElement(hot + "RatePlans", 
							new XElement(hot + "RatePlan",
								// Todo - Reservation - RatePlan depends on customer/res type
								GetRatePlanAttributes(reqData)
							)
						), // RatePlans
						new XElement(hot + "RoomTypes",
							new XElement(hot + "RoomType",
								new XAttribute("roomTypeCode", reqData.RoomTypeCode),
								new XAttribute("numberOfUnits", "1")
							)
						), // RoomTypes
						new XElement(hot + "RoomRates",
							new XElement(hot + "RoomRate",
								new XAttribute("roomTypeCode", reqData.RoomTypeCode),
								new XAttribute("ratePlanCode", reqData.RatePlanCode)
							)
						), // RoomRates
						new XElement(hot + "GuestCounts",
							new XElement(hot + "GuestCount",
								new XAttribute("ageQualifyingCode", "ADULT"),
								new XAttribute("count", reqData.Adults)
							),
							new XElement(hot + "GuestCount",
								new XAttribute("ageQualifyingCode", "CHILD"),
								new XAttribute("count", reqData.ChildCount)
							)
						), // GuestCounts
						GetExtras(reqData.Extras),
						GetStartEnd(reqData.Arrive, reqData.Depart),
						//GetGuarantee(hot, "", reqData),
						new XElement(hot + "HotelReference",
							new XAttribute("chainCode", config.ChainCode),
							new XAttribute("hotelCode", reqData.HotelCode)
						),
						GetResGuestRPHs(reqData),
						GetComments(reqData.Comments),
						new XElement(hot + "Guarantee", new XAttribute("guaranteeType", "GTEED")) // Todo - FIT - Reservation - Guarantee GTEED
					) // RoomStay
				), // RoomStays
				xElementGuests
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, cr, OwsService.Reservation, config);

		return env;
	}

	private XElement GetExtras(List<OwsReservationExtra> extras)
	{
		if (extras.Count == 0)
			return null;

		var list = new List<XElement>();

		foreach (var c in extras)
		{
			list.Add(new XElement(hot + "Package",
				new XAttribute("quantity", $"{c.Quantity}"),
				new XAttribute("packageCode", c.PackageCode)
			));
		}

		return new XElement(hot + "Packages", list);
	}

	public XElement UpdatePackages(OwsPackageRequest reqData, OwsConfig config) //(OwsExtra reqData)
	{
		var cr = new XElement(res + "UpdatePackagesRequest",
			new XAttribute(XNamespace.Xmlns + "res", res),
			new XElement(res + "HotelReference",
				new XAttribute("chainCode", config.ChainCode),
				new XAttribute("hotelCode", reqData.HotelCode)
			),
			GetStartEnd(reqData.Arrive, reqData.Depart),
			new XElement(res + "LegNumber",
				new XAttribute("type", "INTERNAL"),
				reqData.ReservationId
			),
			new XElement(res + "ProductCode",
				reqData.PackageCode
			),
			new XElement(res + "QuantitySpecified",
				"false"
			)
		);
                
		var env = _soapEnvelopeService.GetEnvelope(reqData, cr, OwsService.Reservation, config);

		return env;
	}

	public XElement AddBookingComments(OwsAddBookingCommentRequest reqData, OwsConfig config)
	{
		var cr = new XElement(res + "GuestRequestsRequest",
			new XAttribute(XNamespace.Xmlns + "res", res),
			new XElement(res + "HotelReference",
				new XAttribute("chainCode", config.ChainCode),
				new XAttribute("hotelCode", reqData.HotelCode)
			),
			new XElement(res + "ConfirmationNumber",
				new XAttribute("type", "INTERNAL"),
				reqData.ReservationId
			),
			new XElement(res + "ActionType", "ADD"),
			new XElement(res + "RequestType", "COMMENTS"),
			new XElement(res + "GuestRequests", GetComments(reqData.Comments)
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, cr, OwsService.Reservation, config);

		return env;
	}

	public XElement AddPayment(OwsAddPaymentRequest reqData, OwsConfig config)
	{
		var cr = new XElement(resAdv + "MakePaymentRequest",
			new XAttribute(XNamespace.Xmlns + "res", resAdv),
			new XAttribute(XNamespace.Xmlns + "com", com),
			new XElement(resAdv + "Posting",
				new XAttribute("Charge", $"{reqData.ChargeAmount:0.00}"),
				new XAttribute("LongInfo", reqData.LongInfo),
				new XAttribute("PostDate", reqData.PostDate),
				new XAttribute("PostTime", reqData.PostTime),
				new XElement(resAdv + "ReservationRequestBase",
					new XElement(resAdv + "HotelReference",
						new XAttribute("chainCode", config.ChainCode),
						new XAttribute("hotelCode", reqData.HotelCode)
					),
					new XElement(resAdv + "ReservationID",
						new XElement(com + "UniqueID",
							new XAttribute("source", "RESV_NAME_ID"),
							new XAttribute("type", "INTERNAL"),
							reqData.ResvId)
					)
				)
			),
			new XElement(resAdv + "CreditCardInfo",
				new XElement(resAdv + "CreditCardApproved", 
					new XAttribute("cardType", reqData.OwsPaymentCode))
			),
			new XElement(resAdv + "Reference", reqData.Reference)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, cr, OwsService.Reservation, config);

		return env;
	}

	private List<XAttribute> GetRatePlanAttributes(OwsReservationRequest reqData)
	{
		var list = new List<XAttribute>();

		switch (reqData.AvailabilityMode)
		{
			case AvailabilityMode.Standard:
			case AvailabilityMode.Offer:
				list.Add(new XAttribute("ratePlanCode", reqData.RatePlanCode));
				break;

			case AvailabilityMode.Promotion:
				list.Add(new XAttribute("ratePlanCode", reqData.RatePlanCode));
				list.Add(new XAttribute("promotionCode", reqData.SearchCode));
				break;

			case AvailabilityMode.Company:
				list.Add(new XAttribute("ratePlanCode", reqData.RatePlanCode));
				list.Add(new XAttribute("qualifyingIdType", "CORPORATE"));
				list.Add(new XAttribute("qualifyingIdValue", reqData.SearchCode));
				break;

			case AvailabilityMode.FitAgent:
				list.Add(new XAttribute("ratePlanCode", reqData.RatePlanCode));
				list.Add(new XAttribute("qualifyingIdType", "TRAVEL_AGENT"));
				list.Add(new XAttribute("qualifyingIdValue", reqData.QualifyingIdValue));
				break;

			case AvailabilityMode.Corporate:
				throw new InvalidDataException("Corporate is not supported");

			default:
				//WbsLogger.Error(LogCodes.RatePlanCode, new Exception($"Unexpected availability mode - not stopping progress. {reqData.AvailabilityMode} for rate plan code {reqData.RatePlanCode}"));
				list.Add(new XAttribute("ratePlanCode", reqData.RatePlanCode));
				break;
		}

		return list;
	}
	private XElement GetResGuests(OwsReservationRequest reqData)
	{
		var list = new List<XElement>();

		var guestIndex = 0;
		foreach (var g in reqData.Guests)
		{
			var leadGuest = guestIndex == 0;
			// Todo - FIT - Agent v Guest Profile
			var isBooker = leadGuest 
			               && reqData.RoomIndex == 1 
			               && reqData.CustomerType != CustomerTypes.FitAgent ;

			switch (g.ContactType)
			{
				case ContactTypes.Adult:
					if(leadGuest)
						list.Add(AddGuest(guestIndex, g, leadGuest, isBooker, reqData.AgentUserProfileId));
					break;

				case ContactTypes.Child: //Adding child age details as a comment, since OWS doesn't support guest child ages.
					reqData.Comments.Add(new OwsComment
					{
						Text = $"CHILD AGE: Child Age Guest {guestIndex + 1} is {g.Age}; Name: {g.FirstName}",
						GuestViewable = true
					});

					break;
				case ContactTypes.Infant:
					reqData.Comments.Add(new OwsComment
					{
						Text = $"INFANT AGE: Infant Age Guest {guestIndex + 1} is {g.Age}; COT REQUIRED: {(g.CotRequired ? "YES" : "NO")}; Name: {g.FirstName}",
						GuestViewable = true
					});

					break;
			}
			guestIndex++;
		}
		return new XElement(res1 + "ResGuests", list);
	}

	private XElement AddGuest(int guestIndex, OwsReservationGuest g, bool leadGuest, bool isBooker, string agentUserProfileId)
	{
		return new XElement(res1 + "ResGuest",
			new XAttribute("resGuestRPH", $"{guestIndex}"),
			new XAttribute("ageQualifyingCode", GetMicrosGuestType(g.ContactType)), // Lead guest in a room is always an adult
			new XElement(res1 + "Profiles",
				new XElement(name + "Profile",
					new XElement(name + "Customer",
						new XElement(name + "PersonName",
							new XElement(com + "nameTitle", g.Title),
							new XElement(com + "firstName", g.FirstName),
							new XElement(com + "lastName", g.LastName)
						) // PersonName
					), // Customer
					isBooker ? GetGuestAddress(g) : null,
					isBooker ? GetGuestPhones(g) : null,
					isBooker ? GetGuestEmails(g) : null
					//isBooker ? GetPrivacy() : null // Todo - FIT - Reservation - Privacy
				), // Profile

				// CONTACT Profile links agent user to booking. Note the Agent ProfileID is not required because it's included via qualifying value
				!string.IsNullOrEmpty(agentUserProfileId) ? GetAgentUserProfile(agentUserProfileId) : null
			) // Profiles
		);
	}

	private XElement GetAgentUserProfile(string agentUserProfileId)
	{
		return new XElement(name + "Profile",
			new XElement(name + "Company",
				new XElement(name + "CompanyType", "CONTACT"),
				new XElement(name + "CompanyID", agentUserProfileId)));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	private XElement GetPrivacy() => // Todo - FIT - Reservation - Privacy settings
		new XElement(name + "Privacy",
			GetPrivacyOption("Promotions"),
			GetPrivacyOption("MarketResearch"),
			GetPrivacyOption("ThirdParties"),
			GetPrivacyOption("LoyaltyProgram"),
			GetPrivacyOption("Privacy"),
			GetPrivacyOption("Email"),
			GetPrivacyOption("Phone"),
			GetPrivacyOption("SMS")
		);

	private XElement GetPrivacyOption(string optionType) =>
		new XElement(name + "PrivacyOption",
			new XAttribute("PrivacyOption", optionType),
			new XAttribute("OptionValue", "YES")
		);

	// Only for the lead guest of the first room ... or booker?
	private XElement GetGuestAddress(OwsReservationGuest g) =>
		g.Address == null
			? null
			: new XElement(name + "Addresses",
				new XElement(name + "NameAddress",
					new XElement(com + "AddressLine", g.Address.AddressLine1),
					new XElement(com + "AddressLine", g.Address.AddressLine2),
					new XElement(com + "cityName", g.Address.City),
					new XElement(com + "stateProv", g.Address.StateProvince),
					new XElement(com + "countryCode", g.Address.CountryCode),
					new XElement(com + "postalCode", g.Address.PostalCode)
				)
			);

	private XElement GetGuestPhones(OwsReservationGuest g) =>
		g.Address == null
			? null
			: new XElement(name + "Phones",
				new XElement(name + "NamePhone",
					new XAttribute("phoneType", "HOME"),
					new XAttribute("phoneRole", "PHONE"),
					new XElement(com + "PhoneNumber", g.Address.Telephone)
				),
				new XElement(name + "NamePhone",
					new XAttribute("phoneType", "EMAIL"),
					new XAttribute("phoneRole", "EMAIL"),
					new XElement(com + "PhoneNumber", g.Address.Email)
				)
			);

	private XElement GetGuestEmails(OwsReservationGuest g) =>
		g.Address == null
			? null
			: new XElement(name + "Emails",

				new XElement(name + "NameEmail",
					new XAttribute("primary", "true"),
					g.Address.Email
				)
			);

	private XElement GetGuarantee(string guaranteeText, OwsReservationRequest reqData)
	{
		var guaranteeType = reqData.CustomerType == CustomerTypes.Company || reqData.CustomerType == CustomerTypes.FitAgent
			? "CORP" : "CC";

		return new XElement(hot + "Guarantee",
			new XAttribute("guaranteeType", guaranteeType), 
			new XElement(hot + "GuaranteesAccepted",
				new XElement(hot + "GuaranteeAccepted")
			),
			new XElement(hot + "GuaranteeDescription",
				new XElement(hot + "Text", guaranteeText)
			)
		);
	}

	private XElement GetResGuestRPHs(OwsReservationRequest reqData)
	{
		var list = new List<XElement>();
		for (var i = 0; i < reqData.Guests.Count; i++)
		{
			list.Add(new XElement(hot + "ResGuestRPH", $"{i}"));
		}
		return new XElement( hot + "ResGuestRPHs", list);
	}

	private XElement GetComments(List<OwsComment> comments)
	{
		var list = new List<XElement>();

		foreach (var c in comments)
		{
			list.Add(new XElement(hot + "Comment",
				new XAttribute("commentOriginatorCode", "CRO"), // Todo - Reservation - "commentOriginatorCode", "CRO"?
				new XAttribute("guestViewable", c.GuestViewable.ToString().ToLower()),
				new XElement(hot + "Text", c.Text)
			));
		}

		return new XElement(hot + "Comments", list);
	}

	public XElement FetchBooking(OwsBaseSession reqData, string reservationId, OwsConfig config)
	{
		var el = new XElement(res + "FetchBookingRequest",
			new XAttribute(XNamespace.Xmlns + "res", res),
			new XElement(res + "HotelReference",
				new XAttribute("chainCode", reqData.ChainCode),
				new XAttribute("hotelCode", reqData.HotelCode)
			),
			new XElement(res + "ConfirmationNumber",
				new XAttribute("type", "INTERNAL"),
				reservationId
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, el, OwsService.Reservation, config);

		return env;
	}

	private XElement GetUniqueIdList(OwsReservationRequest reqData)
	{
		return reqData.Modify ? new XElement(res1 + "UniqueIDList",
			new XElement(com + "UniqueID",
				new XAttribute("type", "INTERNAL"),
				reqData.ReservationId
			)
		) : null;
	}

	private XElement GetStartEnd(DateTime arrive, DateTime depart)
	{
		return new XElement(hot + "TimeSpan",
			new XElement(hot + "StartDate", _soapEnvelopeService.GetTimeStamp(arrive)),
			new XElement(hot + "EndDate", _soapEnvelopeService.GetTimeStamp(depart))
		);
	}

	private OwsAgeQualifyingCode GetMicrosGuestType(ContactTypes guestType)
	{
		switch (guestType)
		{
			case ContactTypes.Adult:
				return OwsAgeQualifyingCode.ADULT;
			case ContactTypes.Child:
			case ContactTypes.Infant:
				return OwsAgeQualifyingCode.CHILD;
		}
		return OwsAgeQualifyingCode.ADULT;
	}
}