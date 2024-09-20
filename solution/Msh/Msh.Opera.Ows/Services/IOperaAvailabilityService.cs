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
	
	Task<(OwsRoomStayDetail owsRoomStayDetail, OwsResult owsResult)> GetDetailAvailabilityAsync(OwsAvailabilityRequest reqData);

	Task<(List<OwsPackage> packages, OwsResult owsResult)> FetchPackagesAsync(OwsAvailabilityRequest reqData);
	
}