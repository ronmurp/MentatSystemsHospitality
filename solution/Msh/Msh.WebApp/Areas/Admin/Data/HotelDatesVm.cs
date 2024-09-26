using Msh.Common.Exceptions;
using Msh.Common.Models.Dates;
using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Models.Specials;
using Msh.Loggers.XmlLogger;
using Msh.Opera.Ows.Models;

namespace Msh.WebApp.Areas.Admin.Data;

public class BaseHotelCodeVm
{
	public string Code { get; set; } = string.Empty;
	public string HotelCode { get; set; } = string.Empty;
}
public class HotelDatesVm : BaseHotelCodeVm
{
	public List<ItemDate> Dates { get; set; } = [];
}

public class ItemDatesVm : BaseHotelCodeVm
{
	public List<ItemDate> Dates { get; set; } = [];
}

public class SpecialOptionsVm : BaseHotelCodeVm
{
	public List<SelectOption> Options { get; set; } = [];
}

public class OwsConfigVm
{
	public List<OwsPaymentCodeMap> SchemaMaps { get; set; } = [];
	public List<CriticalErrorTrigger> CriticalErrorTriggers { get; set; } = [];
}

public class LogConfigVm
{
	public List<LogXmlConfigItem> Items { get; set; } = [];
}

public class DiscountErrorsVm : BaseHotelCodeVm
{
	public List<DiscountError> Errors { get; set; } = [];
}