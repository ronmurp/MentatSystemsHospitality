using System.Linq;
using System.Text;
using System.Xml.Linq;
using Msh.Common.Constants;
using Msh.Common.Logger;
using Msh.Common.Models.OwsCommon;
using Msh.Common.Services;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.AvailabilityResponses;
using Msh.Opera.Ows.Models.ReservationRequestModels;
using Msh.Opera.Ows.Services.Config;
using Msh.Opera.Ows.Services.Helpers;

namespace Msh.Opera.Ows.Services.Base;

/// <summary>
/// A base service for building a SOAP envelope and decoding a OwsResult
/// </summary>
public class OperaBaseService
{
	protected readonly OwsConfig _config;
	protected readonly ILogXmlService _logXmlService;
	protected readonly ISwitchListLoader _switchListLoader;
	protected readonly IOwsConfigService _owsConfigService;
	private readonly IOwsPostService _owsPostService;
	private IOwsPostService _owsPostServiceMock = null;

	protected OperaBaseService(OwsConfig config, 
		ILogXmlService logXmlService, 
		ISwitchListLoader switchListLoader, 
		IOwsConfigService owsConfigService,
		IOwsPostService owsPostService)
	{
		_config = config;
		_logXmlService = logXmlService;
		_switchListLoader = switchListLoader;
		_owsConfigService = owsConfigService;
		_owsPostService = owsPostService;
	}

	protected OwsConfig GetConfigOverride(string hotelCodeIn, OwsService service)
	{
            
		var list = _switchListLoader.SwitchList;

		// Use the default config if there's no OwsSources.xml file
		if (list == null) return _config;

		var hotelCode = string.IsNullOrEmpty(hotelCodeIn) ? _config.DefaultHotelCode : hotelCodeIn;

		var opt = list.SingleOrDefault(x => x.HotelCode == hotelCode)?.Services
			.SingleOrDefault(o => o.Service == service);

		if (opt != null)
		{
			//return _owsConfigService.OwsConfigList.FirstOrDefault();

		}

		return _config;
	}

	/// <summary>
	/// Checks for Fault, then checks for FAIL
	/// </summary>
	/// <param name="xdocInput"></param>
	/// <param name="contents"></param>
	/// <param name="responseElement"></param>
	/// <param name="source"></param>
	/// <returns></returns>
	protected (XDocument xdoc, OwsResult owsResult) ParseAndCheckForFail(XDocument xdocInput, string contents, string responseElement, string source)
	{
           
		// Parse and check for Fault ...
		var (xdoc, owsResultFault) = ParseAndCheckForFault(xdocInput, contents);

		if (owsResultFault != null)
			return (null as XDocument, owsResultFault);

		if (xdoc.Descendants(responseElement).FirstOrDefault() == null)
			return (null, OwsResultHelper.WbsResultMessage(
				CommonConst.GdsError.MissingElement,
				$"The main response element is missing: {responseElement}"));

		var owsResult = xdoc.Descendants(responseElement)
			.Select(resp => new 
			{
				OwsResult = GetOwsResult(resp),

			}).SingleOrDefault()?.OwsResult ?? OwsResultHelper.WbsResult;

		return owsResult.ResultStatusFlag == CommonConst.OwsResultStatusFlag.Success
			? (xdoc, null as OwsResult)
			: (null as XDocument, owsResult);

	}

	/// <summary>
	/// The server returns SUCCESS, but the LINQ parsing of the XML returns null
	/// </summary>
	/// <param name="owsDecodedData"></param>
	/// <param name="source"></param>
	protected OwsResult CheckForNoData(object owsDecodedData, string source)
	{
		if (owsDecodedData != null) return null;

		return new OwsResult
		{
			ResultStatusFlag = CommonConst.OwsResultStatusFlag.Fail,
			Text = $"Unexpected null when decoding the XML in {source}",
			OperaErrorCode = CommonConst.OperaErrorCode.XmlDecode,
			GdsError = new GdsError
			{
				ElementId = CommonConst.GdsError.WbsElementId,
				ErrorCode = CommonConst.GdsError.OperaBaseErrorCode
			},
			Source = source
		};

	}

	/// <summary>
	/// Parse, strip namespaces, and check for Fault
	/// </summary>
	/// <param name="xdoc"></param>
	/// <param name="contents"></param>
	/// <returns></returns>
	private (XDocument xdoc, OwsResult owsResult) ParseAndCheckForFault(XDocument xdoc, string contents)
	{
		try
		{
			// Should have failed by now if it cannot already be parsed, but try again
			if(xdoc == null) xdoc = XDocument.Parse(contents);

			xdoc.StripNameSpaces();

			var faultSource = string.Empty;
			var faultCode = string.Empty;
			var faultMessage = string.Empty;
			var faultValue = string.Empty;

			var fault = xdoc.Descendants("Fault")
				.Select(f => new
				{
					FaultCode = f.ValueE("faultcode"),
					FaultString = f.ValueE("faultstring")

				}).SingleOrDefault();

			if (fault == null) return (xdoc, null as OwsResult);

			if (!string.IsNullOrEmpty(fault.FaultCode))
			{
				if (fault.FaultCode.Contains(":"))
				{
					var s = fault.FaultCode.Split(":".ToCharArray());

					if (faultCode.StartsWith("soap"))
					{
						faultSource = s[1];
					}

					if (faultCode.StartsWith("soap") && !string.IsNullOrEmpty(faultSource))
						faultCode = faultSource;
				}
				else
				{
					faultCode = fault.FaultCode;
				}

			}

			if (!string.IsNullOrEmpty(fault.FaultString))
			{
				faultMessage = fault.FaultString;
				if (fault.FaultString.Contains(":"))
				{
					var s = fault.FaultString.Split(":".ToCharArray());
					faultCode = string.IsNullOrEmpty(faultCode) ? s[0] : faultCode;
					faultValue = s[0];
					faultMessage = s.Length > 0 ? s[1].Trim() : string.Empty;
				}
			}

			return (null as XDocument, OwsResultHelper.FaultResult(faultCode, faultSource, faultMessage, faultValue));
		}
		catch (System.Xml.XmlException ex)
		{
			// WbsLogger.Error(LogCodes.OwsOperaParseFault, ex, contents);
			throw ex;
		}
		catch (Exception ex)
		{
			throw ex;
		}
           

	}

	private OwsResult GetOwsResult(XElement resp)
	{
		return resp.Descendants("Result")
			.Select(r => new OwsResult
			{
				// The basic flag which should always be there, except for Fault responses SUCCESS/FAIL
				ResultStatusFlag = r.ValueA("resultStatusFlag"),

				// A text element that on FAIL indicates the reason for the failure
				Text = r.Descendants("TextElement")
					.Select(t => new
					{
						Value = t.ValueE()
					}).SingleOrDefault()?.Value,

				OperaErrorCode = r.ValueE("OperaErrorCode"),

				GdsError = r.Descendants("GDSError")
					.Select(hc => new GdsError
					{
						ElementId = hc.ValueA("elementId"),
						ErrorCode = hc.ValueA("errorCode"),
						ErrorValue = hc.ValueE()
					}).SingleOrDefault()

			}).SingleOrDefault();
	}

	protected OwsCharges GetExtendedCharges(XElement ecs, OperaChargeTypes chargeType)
	{
		return ecs.Descendants($"{chargeType}")
			.Select(rrp => new OwsCharges
			{
				ChargesType = chargeType,
				TotalCharges = rrp.GetSoapAmountNamedAttribute("TotalCharges"),
				Charges = rrp.Descendants("Charges")
					.Select(rrpc => new OperaCharge
					{
						Description = rrpc.ValueE("Description"),
						Amount = rrpc.GetSoapAmount("Amount"),
						CodeType = rrpc.ValueE("CodeType"),
						Code = rrpc.ValueE("Code"),
					}).ToList()
			}).SingleOrDefault();
	}

	protected OwsUniqueId GetOwsUniqueId(XElement uid)
	{
		return new OwsUniqueId
		{
			Type = uid.ValueA("type").Get<OwsUniqueIdType>(),
			Source = uid.ValueA("source"),
			Value = uid.ValueE()
		};
	}

	protected OwsUniqueId GetGuestOwsUniqueId(XElement uid)
	{
		var data = new OwsUniqueId
		{
			Type = uid.ValueA("type").Get<OwsUniqueIdType>(),
			Source = uid.ValueA("source"),
			Value = uid.ValueE()
		};

		return data;
	}

	protected async Task<(XDocument xdoc, string contents, OwsResult owsResult)> PostAsync(StringBuilder sb, string url, string sessionId = "")
	{
		if (_owsPostServiceMock != null)
		{
			return await _owsPostServiceMock.PostAsync(sb, url, sessionId);
		}

		return await _owsPostService.PostAsync(sb, url, sessionId);

	}

	protected (XDocument xdoc, string contents, OwsResult owsResult) PostSync(StringBuilder sb, string url, string sessionKey = "")
	{
		if (_owsPostServiceMock != null)
		{
			return _owsPostServiceMock.PostSync(sb, url, sessionKey);
		}

		return _owsPostService.PostSync(sb, url, sessionKey);
	}

	public void MockOwsPostService(IOwsPostService owsPostService)
	{
		_owsPostServiceMock = owsPostService;
	}
}