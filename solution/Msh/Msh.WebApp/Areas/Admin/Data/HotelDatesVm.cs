using Msh.Common.Exceptions;
using Msh.Common.Models.Dates;
using Msh.HotelCache.Models.Specials;
using Msh.Opera.Ows.Models;

namespace Msh.WebApp.Areas.Admin.Data;

public class HotelDatesVm
{
	public string HotelCode { get; set; } = string.Empty;
	public List<ItemDate> Dates { get; set; } = [];
}

public class ItemDatesVm
{
	public string Code { get; set; } = string.Empty;
	public string HotelCode { get; set; } = string.Empty;
	public List<ItemDate> Dates { get; set; } = [];
}

public class SpecialOptionsVm
{
	public string Code { get; set; } = string.Empty;
	public string HotelCode { get; set; } = string.Empty;
	public List<SelectOption> Options { get; set; } = [];
}

public class OwsConfigVm
{
	public List<OwsPaymentCodeMap> SchemaMaps { get; set; } = [];
	public List<CriticalErrorTrigger> CriticalErrorTriggers { get; set; } = [];
}