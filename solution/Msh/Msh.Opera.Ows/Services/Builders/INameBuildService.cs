using System.Xml.Linq;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

public interface INameBuildService
{
	XElement RegisterNameRequest(OwsUser user, OwsConfig config);

	XElement FetchProfileRequest(OwsBaseSession reqData, string profileId, OwsConfig config);

	/// <summary>
	/// FetchNameRequest
	/// </summary>
	/// <param name="reqData"></param>
	/// <param name="profileId">The (INTERNAL) profile ID of the entity being looked up - only the name is returned. Use FetchProfileRequest</param>
	/// <param name="config"></param>
	/// <returns></returns>
	XElement FetchNameRequest(OwsBaseSession reqData, string profileId, OwsConfig config);

	XElement NameLookupRequestByEmail(OwsBaseSession reqData, string email, OwsConfig config);

	XElement NameLookupRequestByPerson(OwsBaseSession reqData, string firstName, string lastName, string email, OwsConfig config);

	/// <summary>
	/// 
	/// </summary>
	/// <param name="reqData"></param>
	/// <param name="nameToFind"></param>
	/// <param name="nameType">COMPANY, TRAVEL_AGENT, D</param>
	/// <param name="config"></param>
	/// <returns></returns>
	XElement NameLookupRequestByName(OwsBaseSession reqData, string nameToFind, string nameType, OwsConfig config);
}