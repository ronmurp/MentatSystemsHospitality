using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

/// <summary>
/// Build XElement for OWS SOAP Information Service
/// </summary>
public class InformationBuildService: IInformationBuildService
{
	private readonly ISoapEnvelopeService _soapEnvelopeService;

	public InformationBuildService(ISoapEnvelopeService soapEnvelopeService)
	{
		_soapEnvelopeService = soapEnvelopeService;
	}

	public XElement LovQuery2(OwsBaseSession reqData, string requestId, OwsConfig config)
	{
		XNamespace inf = "http://webservices.micros.com/ows/5.1/Information.wsdl";

		var info = new XElement(inf + "LovRequest",
			new XAttribute(XNamespace.Xmlns + "inf", inf),
			new XElement(inf + "LovQuery2",
				new XElement(inf + "LovIdentifier", requestId)
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, info, OwsService.Information, config);

		return env;
	}

	public XElement LovQuery2(OwsBaseSession reqData, LovTypes lovType, OwsConfig config)
	{
		XNamespace inf = "http://webservices.micros.com/ows/5.1/Information.wsdl";

		var info = new XElement(inf + "LovRequest",
			new XAttribute(XNamespace.Xmlns + "inf", inf),
			new XElement(inf + "LovQuery2",
				new XElement(inf + "LovIdentifier", GetLov(lovType))
			)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, info, OwsService.Information, config);

		return env;
	}

	public XElement ChainInformationRequest(OwsBaseSession reqData, OwsConfig config)
	{
		XNamespace inf = "http://webservices.micros.com/ows/5.1/Information.wsdl";

		var info = new XElement(inf + "ChainInformationRequest",
			new XAttribute(XNamespace.Xmlns + "inf", inf),
			new XElement(inf + "LovIdentifier", reqData.ChainCode)
		);

		var env = _soapEnvelopeService.GetEnvelope(reqData, info, OwsService.Information, config);

		return env;
	}

	private string GetLov(LovTypes lovType)
	{
            
		switch (lovType)
		{
			case LovTypes.NameTypes:
				return "NAMETYPE";
			case LovTypes.Countries:
				return "COUNTRYCODES";
			case LovTypes.Titles:
				return "TITLE";
			case LovTypes.AddressTypes:
				return "ADDRESSTYPE";
			case LovTypes.PhoneTypes:
				return "PHONETYPE";
			case LovTypes.PhoneRoles:
				return "PHONEROLE";
			case LovTypes.CreditCardTypes:
				return "CREDITCARDTYPE";
			case LovTypes.PreferenceTypes:
				return "PREFERENCETYPE";
			case LovTypes.PreferenceValues:
				return "PREFERENCEVALUE";
			case LovTypes.FeatureTypes:
				return "FEATURE";
			case LovTypes.CurrencyCodes:
				return "CURRENCYCODE";
			case LovTypes.GuaranteeTypes:
				return "GUARANTEETYPES";

			default:
				throw new Exception("Unsupported LOV type");
		}
	}
}