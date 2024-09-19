using System.Xml.Linq;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

public interface IAvailabilityBuildService
{
	XElement BuildAvailabilityGen(OwsAvailabilityRequest reqData, OwsConfig config);
	XElement BuildAvailabilityDet(OwsAvailabilityRequest reqData, OwsConfig config);
	XElement BuildFetchPackages(OwsAvailabilityRequest reqData, OwsConfig config);
}