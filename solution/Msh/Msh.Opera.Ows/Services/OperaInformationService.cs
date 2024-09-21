using System.Text;
using System.Xml.Linq;
using Msh.Common.Logger;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Cache;
using Msh.Opera.Ows.ExtensionMethods;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Base;
using Msh.Opera.Ows.Services.Builders;

namespace Msh.Opera.Ows.Services;

public class OperaInformationService(
	IOwsCacheService owsCacheService,
	IOwsPostService owsPostService,
	IInformationBuildService informationBuildService,
	ILogXmlService logXmlService)
	: OperaBaseService(logXmlService, owsCacheService, owsPostService),
		IOperaInformationService
{
	protected readonly IInformationBuildService InformationBuildService = informationBuildService;

	public async Task<(OwsBusinessDate owsBusinessDate, OwsResult owsResult)> GetBusinessDateAsync(OwsBaseSession reqData)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = InformationBuildService.LovQuery2(reqData, OwsConst.LovQuery2.BusinessDate, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), LogXmls.OwsBusinessDateReq);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.InformationUrl(), "");

		await _logXmlService.LogXmlText(contents, LogXmls.OwsBusinessDateRes);

		var decode = DecodeOwsBusinessDate(xdoc, contents);

		return (decode.owsBusinessDate, decode.owsResult ?? owsResult);

	}

	public async Task<(List<OwsCountry> countries, OwsResult owsResult)> GetCountryCodesAsync(OwsBaseSession reqData)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = InformationBuildService.LovQuery2(reqData, OwsConst.LovQuery2.CountryCodes, config);
		var sb = new StringBuilder(xElement.ToString());

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.InformationUrl());

		var decode = DecodeOwsCountryCodes(xdoc, contents);

		return (decode.countries, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsChainInformation owsChainInformation, OwsResult owsResult)> GetChainAsync(OwsBaseSession reqData)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = InformationBuildService.ChainInformationRequest(reqData, config);

		var sb = new StringBuilder(xElement.ToString());

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.InformationUrl());

		var decode = DecodeOwsChainCodes(xdoc, contents);

		return (decode.owsChainInformation, decode.owsResult ?? owsResult)!;

	}

	public async Task<(List<InformationItem> information, OwsResult owsResult)> GetLovInformationAsync(OwsBaseSession reqData, LovTypes lovType, string subType)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = InformationBuildService.LovQuery2(reqData, lovType, config);
		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), LogXmls.OwsInfoLovReq);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.InformationUrl());

		await _logXmlService.LogXmlText(contents, LogXmls.OwsInfoLovRes);

		var decode = DecodeLovResponse(xdoc, contents);

		return (decode.information, decode.owsResult ?? owsResult);
	}


	private (OwsBusinessDate owsBusinessDate, OwsResult owsResult) DecodeOwsBusinessDate(XDocument xdocInput, string contents)
	{
		const string mainElement = "LovResponse";
		const string methodName = "DecodeOwsBusinessDate";

		var (xdoc, owsResultFail) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResultFail != null)
			return (null, owsResultFail)!;

		var result = xdoc.Descendants(mainElement)
			.Select(resp => new LovQueryResponse
			{
				OwsBusinessDate = resp.Descendants("LovQueryResult")
					.Select(d => new OwsBusinessDate
					{
						Date = new DateTime(d.ValueA(DateTime.MinValue.Year, "tertiaryQualifierValue"),
							d.ValueA(DateTime.MinValue.Month, "secondaryQualifierValue"),
							d.ValueA(DateTime.MinValue.Day, "qualifierValue"))

					}).SingleOrDefault()

			}).SingleOrDefault();

		var owsResultNull = CheckForNoData(result, methodName);

		return (result?.OwsBusinessDate, owsResultNull)!;

	}

	private (List<OwsCountry> countries, OwsResult owsResult) DecodeOwsCountryCodes(XDocument xdocInput, string contents)
	{
		const string mainElement = "LovResponse";
		const string methodName = "DecodeOwsCountryCodes";

		var (xdoc, owsResultFail) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResultFail != null)
			return (null, owsResultFail)!;

		var result = xdoc.Descendants(mainElement)
			.Select(resp => new LovQueryResponse
			{
				Countries = resp.Descendants("LovValue")
					.Select(d => new OwsCountry
					{
						Name = d.ValueA("description"),
						Code = d.ValueE()

					}).ToList()

			}).SingleOrDefault();

		var owsResultNull = CheckForNoData(result, methodName);

		return (result?.Countries, owsResultNull)!;

	}

	private (List<InformationItem> information, OwsResult owsResult) DecodeLovResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "LovResponse";
		const string methodName = "DecodeLovResponse";

		var (xdoc, owsResultFail) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResultFail != null)
			return (null, owsResultFail);

		var result = xdoc.Descendants(mainElement).Descendants("LovQueryResult").Descendants("LovValue")
			.Select(d => new InformationItem
			{
				Description = d.ValueA("description"),
				Value = d.ValueE()

			}).ToList();

		var owsResultNull = CheckForNoData(result, methodName);

		return (result, owsResultNull);

	}

	private (OwsChainInformation? owsChainInformation, OwsResult owsResult) DecodeOwsChainCodes(XDocument xdocInput, string contents)
	{
		const string mainElement = "ChainInformation";
		const string methodName = "DecodeOwsChainCodes";

		var (xdoc, owsResultFail) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResultFail != null)
			return (null, owsResultFail);

		var result = xdoc.Descendants(mainElement)
			.Select(resp => new
			{
                  
				OwsChainInformation = resp.Descendants(mainElement)
					.Select(d => new OwsChainInformation
					{


					}).SingleOrDefault()

			}).SingleOrDefault();

		var owsResultNull = CheckForNoData(result, methodName);

		return (result?.OwsChainInformation, owsResultNull)!;
	}

}