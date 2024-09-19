using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

/// <summary>
/// Service to build a soap envelope around target element, and to expose OwsConfig
/// </summary>
public interface ISoapEnvelopeService
{
	XElement GetEnvelope(OwsBaseSession reqData, XElement target, OwsService service, OwsConfig config);

	string GetTimeStamp(DateTime dt);
}