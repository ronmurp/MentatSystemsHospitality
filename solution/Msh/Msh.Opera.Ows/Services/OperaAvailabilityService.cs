using System.Text;
using System.Xml.Linq;
using Msh.Common.Logger;
using Msh.Common.Models.OwsCommon;
using Msh.Common.Services;
using Msh.Opera.Ows.ExtensionMethods;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.AvailabilityResponseModels;
using Msh.Opera.Ows.Models.AvailabilityResponses;
using Msh.Opera.Ows.Services.Base;
using Msh.Opera.Ows.Services.Builders;
using Msh.Opera.Ows.Services.Config;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Availability requests: building, sending, parsing result
/// </summary>
public class OperaAvailabilityService : OperaBaseService, IOperaAvailabilityService
{
	private readonly IAvailabilityBuildService _availabilityBuildService;
        
	public string LastRequest { get; private set; }
	public OperaAvailabilityService(IOwsConfigService owsConfigService,
		IOwsPostService owsPostService,
		IAvailabilityBuildService availabilityBuildService, 
		ISwitchListLoader switchListLoader, 
		ILogXmlService logXmlService) : base(
		owsConfigService.OwsConfig, logXmlService, switchListLoader, owsConfigService, owsPostService)
	{
		_availabilityBuildService = availabilityBuildService;
	}

	public (OwsRoomStay owsRoomStay, OwsResult owsResult) GetGeneralAvailability(OwsAvailabilityRequest reqData)
	{
		var config = GetConfigOverride(reqData.HotelCode, OwsService.Availability);

		var xElement = _availabilityBuildService.BuildAvailabilityGen(reqData, config);
		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.ToString();

		_logXmlService.LogXml(xElement, "AvailGenReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = PostSync(sb, config.AvailabilityUrl(), reqData.SessionKey);

		_logXmlService.LogXml(contents, "AvailGenRes", reqData.SessionKey);

		var decode = DecodeOwsGeneralAvailability(xdoc, contents);

		return (decode.roomStay, decode.owsResult ?? owsResult);
	}
	public async Task<(OwsRoomStay owsRoomStay, OwsResult owsResult)> GetGeneralAvailabilityAsync(OwsAvailabilityRequest reqData)
	{
		var config = GetConfigOverride(reqData.HotelCode, OwsService.Availability);

		var xElement = _availabilityBuildService.BuildAvailabilityGen(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.ToString();

		_logXmlService.LogXml(xElement, "AvailGenReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.AvailabilityUrl(), reqData.SessionKey);

		_logXmlService.LogXml(contents, "AvailGenRes", reqData.SessionKey);

		var decode = DecodeOwsGeneralAvailability(xdoc, contents);

		return (decode.roomStay, decode.owsResult ?? owsResult);
	}

	public (OwsRoomStayDetail owsRoomStayDetail, OwsResult owsResult) GetDetailAvailability(OwsAvailabilityRequest reqData)
	{
		var config = GetConfigOverride(reqData.HotelCode, OwsService.Availability);

		var xElement = _availabilityBuildService.BuildAvailabilityDet(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.ToString();
		_logXmlService.LogXmlText(LastRequest, "AvailDetReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = PostSync(sb, config.AvailabilityUrl(), reqData.SessionKey);

		_logXmlService.LogXmlText(contents, "AvailDetRes", reqData.SessionKey);
		var decode = DecodeOwsDetailAvailability(xdoc, contents);

		return (decode.roomStayDetail, decode.owsResult ?? owsResult);
	}


	public async Task<(List<OwsPackage> packages, OwsResult owsResult)> FetchPackagesAsync(OwsAvailabilityRequest reqData)
	{
		var config = GetConfigOverride(reqData.HotelCode, OwsService.Availability);

		var xElement = _availabilityBuildService.BuildFetchPackages(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.ToString();
		_logXmlService.LogXmlText(LastRequest, "FetchPackagesReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.AvailabilityUrl(), reqData.SessionKey);

		_logXmlService.LogXmlText(contents, "FetchPackagesRes", reqData.SessionKey);
		var decode = DecodeOwsPackages(xdoc, contents);

		return (decode.packages, decode.owsResult ?? owsResult);
	}

	public (List<OwsPackage> packages, OwsResult owsResult) FetchPackages(OwsAvailabilityRequest reqData)
	{
		var config = GetConfigOverride(reqData.HotelCode, OwsService.Availability);

		var xElement = _availabilityBuildService.BuildFetchPackages(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		LastRequest = sb.ToString();
		_logXmlService.LogXmlText(LastRequest, "FetchPackagesReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = PostSync(sb, config.AvailabilityUrl(), reqData.SessionKey);

		_logXmlService.LogXmlText(contents, "FetchPackagesRes", reqData.SessionKey);

		var decode = DecodeOwsPackages(xdoc, contents);

		return (decode.packages, decode.owsResult ?? owsResult);
	}

	protected (OwsRoomStay roomStay, OwsResult owsResult) DecodeOwsGeneralAvailability(XDocument xdocInput, string contents)
	{
		const string mainElement = "AvailabilityResponse";
		const string methodName = "DecodeOwsAvailability";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsRoomStay, owsResult);

		var owsRoomStay = xdoc.Descendants("RoomStay")
			.Select(resp => new OwsRoomStay
			{
				// Rate plans only
				OwsRatePlans = resp.Descendants("RatePlan")
					.Select(r => new OwsRatePlan
					{
						RatePlanCode = r.ValueA("ratePlanCode"),
						QualifyingIdType = r.ValueA("qualifyingIdType"),
						QualifyingIdValue = r.ValueA("qualifyingIdValue"),
						PromotionCode = r.ValueA("promotionCode")
					}).ToList(),

				// Room types only
				OwsRoomTypes = resp.Descendants("RoomType")
					.Select(r => new OwsRoomType
					{
						RoomTypeCode = r.ValueA("roomTypeCode"),
						NumberOfUnits = r.ValueA(0, "numberOfUnits")
					}).ToList(),

				// Combined
				OwsRoomRates = resp.Descendants("RoomRate")
					.Select(r => new OwsRoomRate
					{
						RoomTypeCode = r.ValueA("roomTypeCode"),
						RatePlanCode = r.ValueA("ratePlanCode"),
						Total = r.Descendants("Total").Select(x => new
						{
							Total = x.ValueE(0M)
						}).SingleOrDefault()?.Total ?? 0M,
						CurrencyCode = r.Descendants("Total").Select(x => new
						{
							CurrencyCode = x.ValueA("currencyCode")
						}).SingleOrDefault()?.CurrencyCode ?? "GBP"

					}).ToList()

			}).SingleOrDefault();

		var owsResultDecode = CheckForNoData(owsRoomStay, methodName);

		if (owsResultDecode != null)
			return (null as OwsRoomStay, owsResultDecode);

		// Copy number of units from room type to room rate
		foreach (var r in owsRoomStay.OwsRoomRates)
		{
			r.NumberOfUnits = owsRoomStay.OwsRoomTypes.SingleOrDefault(rt => rt.RoomTypeCode == r.RoomTypeCode)
				?.NumberOfUnits ?? 0;
		}

		return (owsRoomStay, new OwsResult(true));
	}

	protected (OwsRoomStayDetail roomStayDetail, OwsResult owsResult) DecodeOwsDetailAvailability(XDocument xdocInput, string contents)
	{
		const string mainElement = "AvailabilityResponse";
		const string methodName = "DecodeOwsAvailability";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsRoomStayDetail, owsResult);

		var roomStayDetail = xdoc.Descendants("RoomStay")
			.Select(resp => new OwsRoomStayDetail
			{
				RatePlanCode = resp.Descendant("RatePlan").ValueA("ratePlanCode"),
				RoomTypeCode = resp.Descendant("RoomType").ValueA("roomTypeCode"),
				OwsRoomRates = resp.Descendants("RoomRate")
					.Select(r => new OwsRoomRateDetail
					{
						RoomTypeCode = r.ValueA("roomTypeCode"),
						RatePlanCode = r.ValueA("ratePlanCode"),
						Total = r.GetSoapAmount("Total"),

						DayRates = r.Descendants("Rates").Descendants("Rate")
							.Select(x => new OwsDateRate
							{
								// This will be in the first, if multiple dates of varying rates
								// If false, then the sole element describes the rate
								RateChangeIndicator = x.ValueA(false, "rateChangeIndicator"),

								// This will be in subsequence elements if varying rates
								EffectiveDate = x.ValueA(DateTime.MinValue, "effectiveDate"),
								Amount = x.GetSoapAmount("Base"),
                                   
							}).ToList()

					}).ToList(),

				ExpectedCharges = resp.Descendants("ExpectedCharges")
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

					}).SingleOrDefault()

			}).SingleOrDefault();

		var owsResultDecode = CheckForNoData(roomStayDetail, methodName);

		if (owsResultDecode != null)
			return (null as OwsRoomStayDetail, owsResultDecode);


		return (roomStayDetail, new OwsResult(true));
	}

	private (List<OwsPackage> packages, OwsResult owsResult) DecodeOwsPackages(XDocument xdocInput, string contents)
	{
		const string mainElement = "FetchAvailablePackagesResponse";
		const string methodName = "DecodeOwsPackages";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null, owsResult);

		var packages = xdoc.Descendants("Package")
			.Select(r => new OwsPackage
			{
				PackageCode = r.ValueA("packageCode"),
				Amount = r.Descendant("Amount").ValueE(0M),
				CurrencyCode = r.Descendant("Amount").ValueA("GBP", "currencyCode"),
				Description = r.Descendant("Description", "Text", "TextElement").ValueE()
			}).ToList();

            
		var owsResultDecode = CheckForNoData(packages, methodName);

		if (owsResultDecode != null)
			return (null, owsResultDecode);

           
		return (packages, null as OwsResult);
	}

      
}