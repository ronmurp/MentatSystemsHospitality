using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services;

public interface IOperaInformationService
{
	Task<(OwsBusinessDate owsBusinessDate, OwsResult owsResult)> GetBusinessDateAsync(OwsBaseSession reqData);

	(OwsBusinessDate owsBusinessDate, OwsResult owsResult) GetBusinessDate(OwsBaseSession reqData);

	(List<InformationItem> information, OwsResult owsResult) GetLovInformation(OwsBaseSession reqData, LovTypes lovType, string subType);

	Task<(List<OwsCountry> countries, OwsResult owsResult)> GetCountryCodesAsync(OwsBaseSession reqData);

	Task<(OwsChainInformation owsChainInformation, OwsResult owsResult)> GetChainAsync(OwsBaseSession reqData);
        
}