using System.Text;
using System.Xml.Linq;
using Msh.Common.Logger;
using Msh.Opera.Ows.ExtensionMethods;
using RestSharp;
using IOwsRepoService = Msh.Opera.Ows.Cache.IOwsRepoService;

namespace Msh.Opera.Ows.Services.CustomTest;

/// <summary>
/// A custom availability service that returns raw data without WBS filtering by RatePlans.xml, RoomTypes.xml etc.
/// </summary>
public class CustomAvailabilityService(IOwsRepoService owsRepoService, ILogXmlService logXmlService)
{
	/// <summary>
	/// Room availability that returns RoomRates with room type number of units added
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <param name="arrive"></param>
	/// <param name="depart"></param>
	/// <param name="adults"></param>
	/// <param name="children"></param>
	/// <param name="qualifyingType"></param>
	/// <param name="qualifyingCode"></param>
	/// <returns></returns>
	public async Task<List<RoomRate>> RunAvailability(string hotelCode, DateTime arrive, DateTime depart,
		int adults, int children,
		string qualifyingType, string qualifyingCode)
	{

		var xdoc = await GetResponseXDocument(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);

		var roomTypes = xdoc.Descendants("RoomType")
			.Select(rr => new RoomType
			{
				RoomTypeCode = rr.ValueA("roomTypeCode"),
				Units = rr.ValueA(0, "numberOfUnits"),

			}).ToList();

		var list = xdoc.Descendants("RoomRate")
			.Select(rr => new RoomRate
			{
				RoomTypeCode = rr.ValueA("roomTypeCode"),
				RatePlanCode = rr.ValueA("ratePlanCode"),
				Rate = decimal.Parse(rr.ValueE("Total") ?? "0.0"),
				Units = roomTypes.SingleOrDefault(rt => rt.RoomTypeCode == rr.ValueA("roomTypeCode"))?.Units ?? 0
			}).ToList();

		return list;

	}

	/// <summary>
	/// Returns only the room types
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <param name="arrive"></param>
	/// <param name="depart"></param>
	/// <param name="adults"></param>
	/// <param name="children"></param>
	/// <param name="qualifyingType"></param>
	/// <param name="qualifyingCode"></param>
	/// <returns></returns>
	public async Task<List<RoomType>> RunAvailabilityRoomTypes(string hotelCode, DateTime arrive, DateTime depart,
		int adults, int children,
		string qualifyingType, string qualifyingCode)
	{

		var xdoc = await GetResponseXDocument(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);

		var roomTypes = xdoc.Descendants("RoomType")
			.Select(rr => new RoomType
			{
				RoomTypeCode = rr.ValueA("roomTypeCode"),
				Units = rr.ValueA(0, "numberOfUnits"),
				Description = rr.Descendant("RoomTypeDescription").ValueE("Text")

			}).ToList();

		return roomTypes;

	}

	/// <summary>
	/// returns only the rate plans
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <param name="arrive"></param>
	/// <param name="depart"></param>
	/// <param name="adults"></param>
	/// <param name="children"></param>
	/// <param name="qualifyingType"></param>
	/// <param name="qualifyingCode"></param>
	/// <returns></returns>
	public async Task<List<RatePlan>> RunAvailabilityRatePlans(string hotelCode, DateTime arrive, DateTime depart,
		int adults, int children,
		string qualifyingType, string qualifyingCode)
	{

		var xdoc =await  GetResponseXDocument(hotelCode, arrive, depart, adults, children, qualifyingType, qualifyingCode);

		var ratePlans = xdoc.Descendants("RatePlan")
			.Select(rr => new RatePlan
			{
				RatePlanCode = rr.ValueA("ratePlanCode"),
				Description = rr.Descendant("RatePlanDescription").ValueE("Text")

			}).ToList();

		return ratePlans;

	}

	private async Task<XDocument> GetResponseXDocument(string hotelCode, DateTime arrive,
		DateTime depart, int adults, int totalChildren,
		string qualifyingType, string qualifyingCode)
	{

		var config = await owsRepoService.GetOwsConfigAsync();

		var url = config.AvailabilityUrl();

		var client = new RestClient(new Uri(url)); // { Timeout = -1 };

		var sb = new StringBuilder();

		var request = await BuildRequestCloud(sb, hotelCode, arrive, depart, adults, totalChildren, qualifyingType, qualifyingCode);

		var response = client.Execute(request);

		var xdoc = XDocument.Parse(response?.Content ?? string.Empty);

		xdoc.StripNameSpaces();

		await logXmlService.LogXmlText(xdoc.ToString(), LogXmls.OwsAvailGenReq);

		return xdoc;
	}

	private async Task<RestRequest> BuildRequestCloud(StringBuilder sb, string hotelCode, DateTime arrive, DateTime depart,
		int adults, int children,
		string qualifyingType, string qualifyingCode)
	{
		var now = DateTime.Now.ToUniversalTime();

		var transactionId = Guid.NewGuid().ToString("N");
		var transactionIdTimeStamp = Guid.NewGuid().ToString("N");
		var timeStamp1 = GetTimeStamp(now);
		var timeStamp2 = GetTimeStamp(now.AddMinutes(2));

		var arriveTime = GetTimeStamp(arrive.Date);
		var departTime = GetTimeStamp(depart.Date);
		var config = await owsRepoService.GetOwsConfigAsync();
		var password = config.Password;
		var userName = config.ElhUserId;

		sb.AppendLine(
				$"<soapenv:Envelope xmlns:core=\"http://webservices.micros.com/og/4.3/Core/\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">");
		sb.AppendLine($"<soapenv:Header>");
		sb.AppendLine($"<wsse:Security soapenv:mustUnderstand=\"1\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">");
		sb.AppendLine($"<wsu:Timestamp wsu:Id=\"{transactionIdTimeStamp}\">");
		sb.AppendLine($"<wsu:Created>{timeStamp1}</wsu:Created>");
		sb.AppendLine($"<wsu:Expires>{timeStamp2}</wsu:Expires>");
		sb.AppendLine($"</wsu:Timestamp>");
		sb.AppendLine($"<wsse:UsernameToken>");
		sb.AppendLine($"<wsse:Username>{userName}</wsse:Username>");
		sb.AppendLine($"<wsse:Password>{password}</wsse:Password>");
		sb.AppendLine($"</wsse:UsernameToken>");
		sb.AppendLine($"</wsse:Security>");
		sb.AppendLine($"<core:OGHeader primaryLangID=\"E\" timeStamp=\"{timeStamp1}\" transactionID=\"WBS{transactionId}\">");
		sb.AppendLine($"<core:Origin entityID=\"OWS\" systemType=\"WEB\"/>");
		sb.AppendLine($"<core:Destination entityID=\"OWS\" systemType=\"PMS\"/>");
		sb.AppendLine($"<core:Authentication>");
		sb.AppendLine($"<core:UserCredentials>");
		sb.AppendLine($"<core:UserName>{userName}@I{hotelCode}</core:UserName>");
		sb.AppendLine($"<core:Domain>{hotelCode}</core:Domain>");
		sb.AppendLine($"</core:UserCredentials>");
		sb.AppendLine($"</core:Authentication>");
		sb.AppendLine($"</core:OGHeader>");
		sb.AppendLine($"</soapenv:Header>");
		sb.AppendLine($"<soapenv:Body>");
		sb.AppendLine($"<avwsdl:AvailabilityRequest summaryOnly=\"true\" xmlns:av=\"http://webservices.micros.com/og/4.3/Availability/\" xmlns:avwsdl=\"http://webservices.micros.com/ows/5.1/Availability.wsdl\" xmlns:hot=\"http://webservices.micros.com/og/4.3/HotelCommon/\">");
		sb.AppendLine($"<av:AvailRequestSegment availReqType=\"Room\" numberOfAdults=\"{adults}\" numberOfChildren=\"{children}\" numberOfRooms=\"1\">");
		sb.AppendLine($"<av:StayDateRange>");
		sb.AppendLine($"<hot:StartDate>{arriveTime}</hot:StartDate>");
		sb.AppendLine($"<hot:EndDate>{departTime}</hot:EndDate>");
		sb.AppendLine($"</av:StayDateRange>");
		sb.AppendLine($"<av:HotelSearchCriteria>");
		sb.AppendLine($"<av:Criterion>");
		sb.AppendLine($"<av:HotelRef chainCode=\"ELH\" hotelCode=\"{hotelCode}\"/>");
		sb.AppendLine($"</av:Criterion>");
		sb.AppendLine($"</av:HotelSearchCriteria>");
		if (!string.IsNullOrEmpty(qualifyingType) && !string.IsNullOrEmpty(qualifyingCode))
		{
			sb.AppendLine($"<av:RatePlanCandidates>");
			sb.AppendLine($"<av:RatePlanCandidate qualifyingIdType=\"{qualifyingType}\" qualifyingIdValue=\"{qualifyingCode}\"/>");
			sb.AppendLine($"</av:RatePlanCandidates>");
		}
		sb.AppendLine($"</av:AvailRequestSegment>");
		sb.AppendLine($"</avwsdl:AvailabilityRequest>");
		sb.AppendLine($"</soapenv:Body>");
		sb.AppendLine($"</soapenv:Envelope>");

		await logXmlService.LogXmlText(sb.ToString(), LogXmls.OwsAvailGenReq);
		
		var request = new RestRequest(new Uri(config.AvailabilityUrl()), Method.Post);
		request.AddHeader("Content-Type", "text/xml");
		request.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

		return request;
	}


	private async Task<RestRequest> BuildRequestSystem(StringBuilder sb, string hotelCode, DateTime arrive, DateTime depart,
		int adults, int children)
	{
		var transactionId = Guid.NewGuid().ToString("N");

		var timeStamp = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss}";

		sb.AppendLine($"<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		sb.AppendLine($"<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
		sb.AppendLine($"<soap:Header>");
		sb.AppendLine($"<OGHeader transactionID=\"{transactionId}\" primaryLangID=\"E\" timeStamp=\"{timeStamp}\" xmlns=\"http://webservices.micros.com/og/4.3/Core/\">");
		sb.AppendLine($"<Origin entityID=\"OWS\" systemType=\"WEB\" />");
		sb.AppendLine($"<Destination entityID=\"CHA\" systemType=\"PMS\" />");
		sb.AppendLine($"</OGHeader>");
		sb.AppendLine($"</soap:Header>");
		sb.AppendLine($"<soap:Body>");
		sb.AppendLine($"<AvailabilityRequest xmlns:a=\"http://webservices.micros.com/og/4.3/Availability/\" xmlns:hc=\"http://webservices.micros.com/og/4.3/HotelCommon/\" summaryOnly=\"true\" xmlns=\"http://webservices.micros.com/ows/5.1/Availability.wsdl\">");
		sb.AppendLine($"<a:AvailRequestSegment availReqType=\"Room\" numberOfRooms=\"1\" totalNumberOfGuests=\"{adults}\" numberOfChildren=\"{children}\" >");
		sb.AppendLine($"<a:StayDateRange>");
		sb.AppendLine($"<hc:StartDate>{arrive:yyyy-MM-dd}</hc:StartDate>");
		sb.AppendLine($"<hc:EndDate>{depart:yyyy-MM-dd}</hc:EndDate>");
		sb.AppendLine($"</a:StayDateRange>");
		sb.AppendLine($"<a:HotelSearchCriteria>");
		sb.AppendLine($"<a:Criterion>");
		sb.AppendLine($"<a:HotelRef chainCode=\"CHA\" hotelCode=\"{hotelCode}\" />");
		sb.AppendLine($"</a:Criterion>");
		sb.AppendLine($"</a:HotelSearchCriteria>");
		sb.AppendLine($"</a:AvailRequestSegment>");
		sb.AppendLine($"</AvailabilityRequest>");
		sb.AppendLine($"</soap:Body>");
		sb.AppendLine($"</soap:Envelope>");

		await logXmlService.LogXmlText(sb.ToString(), LogXmls.OwsAvailGenReq);

		var request = new RestRequest(new Uri(""), Method.Post);
		request.AddHeader("Content-Type", "text/xml");
		request.AddHeader("SOAPAction", "http://webservices.micros.com/ows/5.1/Availability.wsdl#Availability");
		request.AddParameter("text/xml", sb.ToString(), ParameterType.RequestBody);

		return request;
	}

	private string GetTimeStamp(DateTime dt) => $"{dt:yyyy-MM-ddTHH:mm:ss.fff}Z";

	public class RoomRate
	{
		public string? RoomTypeCode { get; set; }
		public string? RatePlanCode { get; set; }
		public int Units { get; set; }
		public decimal Rate { get; set; }
	}

	public class RoomType
	{
		public string? RoomTypeCode { get; set; }
		public int Units { get; set; }
		public string Description { get; set; } = string.Empty;

	}

	public class RatePlan
	{
		public string? RatePlanCode { get; set; }
		public string Description { get; set; } = string.Empty;
	}
}