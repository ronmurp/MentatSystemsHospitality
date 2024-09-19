using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

public class SecurityBuildService : ISecurityBuildService
{
	private readonly ISoapEnvelopeService _soapEnvelopeService;

	private readonly XNamespace sec = "http://webservices.micros.com/ows/5.1/Security.wsdl";
	public SecurityBuildService(ISoapEnvelopeService soapEnvelopeService)
	{
		_soapEnvelopeService = soapEnvelopeService;
	}
	public XElement AuthenticateUserRequest(OwsUser user, OwsConfig config)
	{
		var xElement = new XElement(sec + "AuthenticateUserRequest",
			new XAttribute(XNamespace.Xmlns + "sec", sec),
			new XAttribute("membershipNumber", user.LoginName),
			new XAttribute("password", user.Password)
		);

		user.HotelCode = config.DefaultHotelCode;

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Security, config);

		return env;
	}

	public XElement CreateUserRequest(OwsUser user, OwsConfig config)
	{
		var xElement = new XElement(sec + "CreateUserRequest",
			new XAttribute(XNamespace.Xmlns + "sec", sec),
			new XAttribute("loginName", user.LoginName),
			new XAttribute("password", user.Password),
			new XElement(sec + "NameID",
				new XAttribute("type", "INTERNAL"),
				user.ProfileId
			)
		);

		user.HotelCode = config.DefaultHotelCode;

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Security, config);

		return env;
	}

	public XElement FetchQuestionListRequest(OwsUser user, OwsConfig config)
	{
		var xElement = new XElement(sec + "FetchQuestionListRequest",
			new XAttribute(XNamespace.Xmlns + "sec", sec)
		);

		user.HotelCode = config.DefaultHotelCode;

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Security, config);

		return env;
	}

	public XElement ResetPasswordRequest(OwsUser user, OwsConfig config)
	{
		var xElement = new XElement(sec + "ResetPasswordRequest",
			new XAttribute(XNamespace.Xmlns + "sec", sec),
			new XElement(sec + "LoginCredentials",
				new XAttribute("loginName", user.LoginName),
				new XAttribute("password", user.Password)
			),
			new XElement(sec + "Question",
				new XAttribute("questionId", user.QuestionId)
			),
			new XElement(sec + "Answer",
				user.QuestionAnswer
			)
		);

		user.HotelCode = config.DefaultHotelCode;

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Security, config);

		return env;
	}

	public XElement UpdateQuestionRequest(OwsUser user, OwsConfig config)
	{
		var xElement = new XElement(sec + "UpdateQuestionRequest",
			new XAttribute(XNamespace.Xmlns + "sec", sec),
			new XElement(sec + "LoginCredentials",
				new XAttribute("loginName", user.LoginName),
				new XAttribute("password", user.Password)
			),
			new XElement(sec + "Question",
				new XAttribute("questionId", user.QuestionId)
			),
			new XElement(sec + "Answer",
				user.QuestionAnswer
			)
		);

		user.HotelCode = config.DefaultHotelCode;

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Security, config);

		return env;
	}

	public XElement UpdatePasswordRequest(OwsUser user, string oldPassword, string newPassword, OwsConfig config)
	{
		var xElement = new XElement(sec + "UpdatePasswordRequest",
			new XAttribute(XNamespace.Xmlns + "sec", sec),
			new XAttribute("membershipNumber", user.LoginName),
			new XAttribute("oldPassword", oldPassword),
			new XAttribute("newPassword", newPassword)
		);

		user.HotelCode = config.DefaultHotelCode;

		var env = _soapEnvelopeService.GetEnvelope(user, xElement, OwsService.Security, config);

		return env;
	}
}