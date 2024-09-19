using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

/// <summary>
/// Build XElement for OWS SOAP Information Service
/// </summary>
public interface IInformationBuildService
{
	XElement LovQuery2(OwsBaseSession reqData, string requestId, OwsConfig config);
	XElement LovQuery2(OwsBaseSession reqData, LovTypes lovType, OwsConfig config);
	XElement ChainInformationRequest(OwsBaseSession reqData, OwsConfig config);
}