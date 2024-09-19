using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

public class NameBuildService : INameBuildService
{
	private readonly ISoapEnvelopeService _soapEnvelopeService;

	private readonly XNamespace name = "http://webservices.micros.com/ows/5.1/Name.wsdl";
	private readonly XNamespace hot = "http://webservices.micros.com/og/4.3/HotelCommon/";
	private readonly XNamespace nm = "http://webservices.micros.com/og/4.3/Name/";
	private readonly XNamespace com = "http://webservices.micros.com/og/4.3/Common/";

	public NameBuildService(ISoapEnvelopeService soapEnvelopeService)
	{
		_soapEnvelopeService = soapEnvelopeService;
	}

	public XElement RegisterNameRequest(OwsUser user, OwsConfig config)
	{
		var xElement = new XElement(name + "RegisterNameRequest",
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "nm", nm),
			new XAttribute(XNamespace.Xmlns + "com", com),
			new XElement(name + "PersonName",
				new XElement(com + "title", user.Title),
				new XElement(com + "firstName", user.FirstName),
				new XElement(com + "lastName", user.LastName)
			),
			new XElement(name + "Email", user.Email)
		);

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Name, config);

		return env;
	}

	public XElement FetchProfileRequest(OwsBaseSession reqData, string profileId, OwsConfig config)
	{
		var xElement = new XElement(name + "FetchProfileRequest",
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "nm", nm),
			new XElement(name + "NameID",
				new XAttribute("type", "INTERNAL"),
				profileId
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, xElement, OwsService.Name, config);

		return env;
	}

	/// <summary>
	/// FetchNameRequest
	/// </summary>
	/// <param name="reqData"></param>
	/// <param name="profileId">The (INTERNAL) profile ID of the entity being looked up - only the name is returned. Use FetchProfileRequest</param>
	/// <param name="config"></param>
	/// <returns></returns>
	public XElement FetchNameRequest(OwsBaseSession reqData, string profileId, OwsConfig config)
	{
		var xElement = new XElement(name + "FetchNameRequest",
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "nm", nm),
			new XElement(name + "NameID",
				new XAttribute("type", "INTERNAL"),
				profileId
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData,  xElement, OwsService.Name, config);

		return env;
	}

	public XElement NameLookupRequestByEmail(OwsBaseSession reqData, string email, OwsConfig config)
	{
		var xElement = new XElement(name + "NameLookupRequest",
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "nm", nm),
			new XElement(name + "NameLookupCriteria",
				new XElement(nm + "EmailAddress",
					new XElement(nm + "EmailAddress",
						email
					)
				)
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, xElement, OwsService.Name, config);

		return env;
	}

	public XElement NameLookupRequestByPerson(OwsBaseSession reqData, string firstName, string lastName, string email, OwsConfig config)
	{
		var elFirstName = string.IsNullOrEmpty(firstName) ? null : new XElement(nm + "FirstName", firstName);
		var elLastName = string.IsNullOrEmpty(lastName) ? null : new XElement(nm + "LastName", lastName);
		var elEmail = string.IsNullOrEmpty(email) ? null : new XElement(nm + "EmailAddress", email);

		var xElement = new XElement(name + "NameLookupRequest",
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "nm", nm),
			new XElement(name + "NameLookupCriteria",
				new XElement(nm + "NameLookup",
					elFirstName,
					elLastName, 
					elEmail
				)
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, xElement, OwsService.Name, config);

		return env;
	}

	public XElement NameLookupRequestByName(OwsBaseSession reqData, string nameToFind, string nameType, OwsConfig config)
	{
		var xElement = new XElement(name + "NameLookupRequest",
			new XAttribute(XNamespace.Xmlns + "name", name),
			new XAttribute(XNamespace.Xmlns + "nm", nm),
			new XElement(name + "NameLookupCriteria",
				new XElement(nm + "Name",
					new XElement(nm + "Name",
						nameToFind
					),
					new XElement(nm + "NameType",
						nameType
					)
				)
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, xElement, OwsService.Name, config);

		return env;
	}
}