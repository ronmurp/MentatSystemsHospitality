using Microsoft.AspNetCore.Mvc;
using Msh.Common.Logger;
using Msh.Common.Models.ViewModels;
using Msh.Loggers.XmlLogger;
using Msh.Opera.Ows.Models;
using Msh.WebApp.Areas.Admin.Data;

namespace Msh.WebApp.API.Loggers;

[ApiController]
[Route("api/loggersapi")]
public class LoggersApiController(ILogXmlRepoService logXmlRepoService) : Controller
{

	[HttpGet]
	[Route("LogXmlConfigEditItems/{group}")]
	public async Task<IActionResult> LogXmlConfigEditItems(string group)
	{
		var owsConfig = await logXmlRepoService.GetConfig(group);
		
		if (owsConfig != null)
		{
			return Ok(new ObjectVm
			{
				Data = owsConfig.Items
			});
		}

		return Ok(new ObjectVm
		{
			Data = new List<OwsPaymentCodeMap>()
		});
	}

	[HttpPost]
	[Route("LogXmlConfigEditItems/{group}")]
	public async Task<IActionResult> LogXmlConfigEditItems([FromBody] LogConfigVm data, string group)
	{
		try
		{
			await Task.Delay(0);

			var owsConfig = await logXmlRepoService.GetConfig(group);
			owsConfig.Items = data.Items;

			await logXmlRepoService.SaveConfig(owsConfig, group);

			return Ok(new ObjectVm
			{
				Data = owsConfig.Items
			});
		}
		catch (Exception ex)
		{
			return Ok(new ObjectVm
			{
				Success = false,
				UserErrorMessage = ex.Message
			});
		}
	}


	[HttpPost]
	[Route("LogXmlConfigEditItemsInit/{group}")]
	public async Task<IActionResult> LogXmlConfigEditItemsInit(string group)
	{
		try
		{
			await Task.Delay(0);
			var list = new List<LogXmlConfigItem>();

			var owsConfig = await logXmlRepoService.GetConfig(group);

			foreach (var e in Enum.GetValues<LogXmls>().Cast<LogXmls>().Where(x => $"{x}".StartsWith(group)))
			{
				var eStr = $"{e}";
				if (owsConfig.Items.All(x => x.Key != eStr))
					list.Add(new LogXmlConfigItem
					{
						Key = eStr
					});
			}
			

			owsConfig.Items.AddRange(list);

			await logXmlRepoService.SaveConfig(owsConfig, group);

			return Ok(new ObjectVm
			{
				Data = owsConfig.Items
			});
		}
		catch (Exception ex)
		{
			return Ok(new ObjectVm
			{
				Success = false,
				UserErrorMessage = ex.Message
			});
		}
	}

}