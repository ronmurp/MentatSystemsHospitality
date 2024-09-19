using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.AvailabilityResponseModels;
using Msh.Opera.Ows.Models.AvailabilityResponses;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Availability requests: building, sending, parsing result
/// </summary>
public interface IOperaAvailabilityService
{
	string LastRequest { get; }

	Task<(OwsRoomStay owsRoomStay, OwsResult owsResult)> GetGeneralAvailabilityAsync(OwsAvailabilityRequest reqData);
	(OwsRoomStay owsRoomStay, OwsResult owsResult) GetGeneralAvailability(OwsAvailabilityRequest reqData);


	(OwsRoomStayDetail owsRoomStayDetail, OwsResult owsResult) GetDetailAvailability(OwsAvailabilityRequest reqData);

	Task<(List<OwsPackage> packages, OwsResult owsResult)> FetchPackagesAsync(OwsAvailabilityRequest reqData);
	(List<OwsPackage> packages, OwsResult owsResult) FetchPackages(OwsAvailabilityRequest reqData);

	void MockOwsPostService(IOwsPostService owsPostService);
}