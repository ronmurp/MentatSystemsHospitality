using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

public class AvailabilityBuildService : IAvailabilityBuildService
{
	XNamespace avwsdl = "http://webservices.micros.com/ows/5.1/Availability.wsdl";
	XNamespace av = "http://webservices.micros.com/og/4.3/Availability/";
	XNamespace hot = "http://webservices.micros.com/og/4.3/HotelCommon/";
	XNamespace mem = "http://webservices.micros.com/og/4.3/Membership/";

	private readonly ISoapEnvelopeService _soapEnvelopeService;

	public AvailabilityBuildService(ISoapEnvelopeService soapEnvelopeService)
	{
		_soapEnvelopeService = soapEnvelopeService;
	}

	public XElement BuildAvailabilityGen(OwsAvailabilityRequest reqData, OwsConfig config)
	{
		return BuildAvailability(reqData, true, config);
	}

	public XElement BuildAvailabilityDet(OwsAvailabilityRequest reqData, OwsConfig config)
	{
		return BuildAvailability(reqData, false, config);
	}

	public XElement BuildFetchPackages(OwsAvailabilityRequest reqData, OwsConfig config)
	{
		var xElement = new XElement(avwsdl + "FetchAvailablePackagesRequest",
			new XAttribute(XNamespace.Xmlns + "avwsdl", avwsdl),
			new XAttribute(XNamespace.Xmlns + "av", av),
			new XAttribute(XNamespace.Xmlns + "hot", hot),
			new XElement(avwsdl + "HotelReference",
				new XAttribute("chainCode", config.ChainCode),
				new XAttribute("hotelCode", reqData.HotelCode)
			),
			new XElement(avwsdl + "StayDateRange",
				new XElement(hot + "StartDate", _soapEnvelopeService.GetTimeStamp(reqData.Arrive)),
				new XElement(hot + "EndDate", _soapEnvelopeService.GetTimeStamp(reqData.Depart))
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, xElement, OwsService.Availability, config);

		return env;
	}

	public XElement BuildAvailability(OwsAvailabilityRequest reqData, bool summary, OwsConfig config)
	{
		var summaryOnly = summary ? "true" : "false";

		var xElement = new XElement(avwsdl + "AvailabilityRequest",
			new XAttribute("summaryOnly", summaryOnly),
			new XAttribute(XNamespace.Xmlns + "avwsdl", avwsdl),
			new XAttribute(XNamespace.Xmlns + "av", av),
			new XAttribute(XNamespace.Xmlns + "hot", hot),
			new XElement(av + "AvailRequestSegment",
				new XAttribute("availReqType", "Room"),
				new XAttribute("numberOfRooms", "1"),
				new XAttribute("numberOfAdults", reqData.Adults),
				new XAttribute("numberOfChildren", reqData.ChildCount),
				new XElement(av + "StayDateRange",
					new XElement(hot + "StartDate", _soapEnvelopeService.GetTimeStamp(reqData.Arrive)),
					new XElement(hot + "EndDate", _soapEnvelopeService.GetTimeStamp(reqData.Depart))
				),
				RatePlanCandidate(reqData, !summary),
				summary ? null : RoomStayCandidate(reqData.RoomTypeCode),
				new XElement(av + "HotelSearchCriteria",
					new XElement(av + "Criterion",
						new XElement(av + "HotelRef",
							new XAttribute("chainCode", config.ChainCode),
							new XAttribute("hotelCode", reqData.HotelCode)
						)
					)
				)
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, xElement, OwsService.Availability, config);

		return env;
	}

	private XElement RatePlanCandidate(OwsAvailabilityRequest reqData, bool isDetail)
	{
		switch (reqData.AvailabilityMode)
		{
			case AvailabilityMode.Promotion:
				return new XElement(av + "RatePlanCandidates",
					new XElement(av + "RatePlanCandidate", 
						new XAttribute("promotionCode", reqData.RatePlanCode)
					)
				);

			case AvailabilityMode.Company:
				throw new NotImplementedException("Need qualifyingIdType type");
			//return new XElement(av + "RatePlanCandidates",
			//    new XElement(av + "RatePlanCandidate",
			//        new XAttribute("qualifyingIdType", "COMPANY"),
			//        new XAttribute("qualifyingIdValue", reqData.SearchCode) // Company code
			//    )
			//);

			case AvailabilityMode.Corporate:
				throw new NotImplementedException("Need qualifyingIdType type  etc.");


			// Todo - FIT - Availability - Build request qualifyingIdValue??? qualifyingIdType??? 
			case AvailabilityMode.FitAgent when isDetail:
				return new XElement(av + "RatePlanCandidates",
					new XElement(av + "RatePlanCandidate",
						new XAttribute("ratePlanCode", reqData.RatePlanCode),
						new XAttribute("qualifyingIdType", "TRAVEL_AGENT"),
						new XAttribute("qualifyingIdValue", reqData.SearchCode) // Which agent code or ID?
					)
				);

			// Todo - FIT - Availability - Build request qualifyingIdValue??? qualifyingIdType???
			case AvailabilityMode.FitAgent:
				return new XElement(av + "RatePlanCandidates",
					new XElement(av + "RatePlanCandidate",
						new XAttribute("qualifyingIdType", "TRAVEL_AGENT"),
						new XAttribute("qualifyingIdValue", reqData.SearchCode) // Which agent code or ID?
					)
				);
               

			case AvailabilityMode.Standard when isDetail:
				return new XElement(av + "RatePlanCandidates",
					new XElement(av + "RatePlanCandidate",
						new XAttribute("ratePlanCode", reqData.RatePlanCode)
					)
				);
		}

		return null;
	}

	private XElement RoomTypeCandidate(string roomTypeCode)
	{
		var rpc = new XElement(av + "RoomTypeCandidates",
			new XElement(av + "RoomTypeCandidates",
				new XAttribute("roomTypeCode", roomTypeCode)
			)
		);

		return rpc;
	}

	private XElement RoomStayCandidate(string roomTypeCode)
	{
		var rpc = new XElement(av + "RoomStayCandidates",
			new XElement(av + "RoomStayCandidate",
				new XAttribute("roomTypeCode", roomTypeCode)
			)
		);

		return rpc;
	}

}