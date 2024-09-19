using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services;

public interface IOperaNameService
{
	(OwsProfile owsProfile, OwsResult owsResult) FetchProfile(OwsBaseSession reqData, string profileId);

	Task<(OwsProfile owsProfile, OwsResult owsResult)> FetchProfileAsync(OwsBaseSession reqData, string profileId);

	(List<OwsProfile> owsProfiles, OwsResult owsResult) NameLookupRequest(OwsBaseSession reqData, string email);

	/// <summary>
	/// NameType: COMPANY, TRAVEL_AGENT, D
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <param name="name"></param>
	/// <param name="nameType">COMPANY, TRAVEL_AGENT, D</param>
	/// <returns></returns>
	(List<OwsProfile> owsProfiles, OwsResult owsResult) NameLookupRequestByName(OwsBaseSession reqData, string name, string nameType);

	(List<OwsProfile> owsProfiles, OwsResult owsResult) NameLookupRequestByPerson(OwsBaseSession reqData, string firstName, string lastName, string email);

	(OwsProfile owsProfile, OwsResult owsResult) FetchName(OwsBaseSession reqData, string profileId);

	(OwsProfile owsProfile, OwsResult owsResult) RegisterName(OwsUser user);
}