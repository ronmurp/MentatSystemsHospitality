using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services;

public interface IOperaNameService
{
	Task<(OwsProfile owsProfile, OwsResult owsResult)> FetchProfileAsync(OwsBaseSession reqData, string profileId);

	Task<(List<OwsProfile> owsProfiles, OwsResult owsResult)> NameLookupRequestAsync(OwsBaseSession reqData, string email);

	/// <summary>
	/// NameType: COMPANY, TRAVEL_AGENT, D
	/// </summary>
	/// <param name="reqData"></param>
	/// <param name="name"></param>
	/// <param name="nameType">COMPANY, TRAVEL_AGENT, D</param>
	/// <returns></returns>
	Task<(List<OwsProfile> owsProfiles, OwsResult owsResult)> NameLookupRequestByNameAsync(OwsBaseSession reqData, string name, string nameType);

	Task<(List<OwsProfile> owsProfiles, OwsResult owsResult)> NameLookupRequestByPersonAsync(OwsBaseSession reqData, string firstName, string lastName, string email);

	Task<(OwsProfile owsProfile, OwsResult owsResult)> FetchNameAsync(OwsBaseSession reqData, string profileId);

	Task<(OwsProfile owsProfile, OwsResult owsResult)> RegisterNameAsync(OwsUser user);
}