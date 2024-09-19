using System.Text.Json.Serialization;
using Msh.Common.Models.OwsCommon;

namespace Msh.Common.Models;

public class SwitchOperation
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OwsService Service { get; set; }




	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OperaServiceSource Source { get; set; }
}