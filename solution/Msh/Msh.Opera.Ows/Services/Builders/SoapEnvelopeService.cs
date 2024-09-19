using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

/// <summary>
/// Service to build a soap envelope around target element, and to expose OwsConfig
/// </summary>
public class SoapEnvelopeService : ISoapEnvelopeService
{
	public XElement GetEnvelope(OwsBaseSession reqData, XElement target, OwsService service, OwsConfig config)
	{
		var hotelCode = string.IsNullOrEmpty(reqData.HotelCode) ? config.DefaultHotelCode : reqData.HotelCode;

		XNamespace core = "http://webservices.micros.com/og/4.3/Core/";
		XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

		XElement root = new XElement(soapenv + "Envelope",
			new XAttribute(XNamespace.Xmlns + "soapenv", soapenv),
			new XAttribute(XNamespace.Xmlns + "core", core),
			new XElement(soapenv + "Header",
				GetSecurity(config.ElhUserId, config.Password),
				GetOgHeader(hotelCode, config.ElhUserId, reqData.TransactionId, reqData.SessionKey)
			), // Header
			new XElement(soapenv + "Body",
				target
			)
		);

		return root;
	}

	public XElement GetSecurity(string userName, string password)
	{
		if (string.IsNullOrEmpty(userName))
			return null;
		XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
		XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

		var tsId = Guid.NewGuid().ToString();
		var created = DateTime.Now.ToUniversalTime();
		var expires = created.AddMinutes(3);

		var security = new XElement(wsse + "Security",
			new XAttribute(XNamespace.Xmlns + "wsse", wsse),
			new XAttribute(XNamespace.Xmlns + "wsu", wsu),
			new XElement(wsu + "Timestamp",
				new XAttribute(wsu + "Id", tsId),
				new XElement(wsu + "Created", GetTimeStamp(created)),
				new XElement(wsu + "Expires", GetTimeStamp(expires))
			), // TimeStamp
			new XElement(wsse + "UsernameToken",
				new XElement(wsse + "Username", userName),
				new XElement(wsse + "Password", password)
			) //UsernameToken
		); // Security

		return security;
	}

	// Todo - What happens when we want to search by ReservationId and we don't know which hotel?
	public XElement GetOgHeader(string hotelCode, string userName, string transactionId, string sessionId)
	{
		XNamespace core = "http://webservices.micros.com/og/4.3/Core/";

		XElement ogHeader = new XElement(core + "OGHeader",
			new XAttribute("primaryLangID", "E"),
			new XAttribute("timeStamp", GetTimeStamp(DateTime.Now)),
			new XAttribute("transactionID", transactionId),
			new XAttribute("sessionID", sessionId),
			new XElement(core + "Origin",
				new XAttribute("entityID", "OWS"),
				new XAttribute("systemType", "WEB")
			),
			new XElement(core + "Destination",
				new XAttribute("entityID", "OWS"),
				new XAttribute("systemType", "PMS")
			),
			GetAuthentication(core, hotelCode, userName)
		);

		return ogHeader;
	}

	private XElement GetAuthentication(XNamespace core, string hotelCode, string userName)
	{
		if (string.IsNullOrEmpty(userName))
			return null;

		var fullUserName = $"{userName}@I{hotelCode}";

		var authentication = new XElement(core + "Authentication",
			new XElement(core + "UserCredentials",
				new XElement(core + "UserName", fullUserName),
				new XElement(core + "Domain", hotelCode)
			)
		);

		return authentication;
	}

	public string GetTimeStamp(DateTime dt) => $"{dt:yyyy-MM-ddTHH:mm:ss.fff}Z";

}