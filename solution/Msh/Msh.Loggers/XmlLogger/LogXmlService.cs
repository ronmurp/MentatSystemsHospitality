using Msh.Common.Logger;

namespace Msh.Loggers.XmlLogger;

public class LogXmlService : ILogXmlService
{
	public async Task LogXml(object obj, LogXmls key, string sessionKey = "", IXmlRedactor? redactor = null)
	{
		if (obj is string xml)
			await LogXmlText(xml, key, sessionKey, redactor);
		else
			await LogXmlObject(obj, key, sessionKey, redactor);
	}

	public async Task LogXmlText(string xml, LogXmls key, string sessionKey = "", IXmlRedactor? redactor = null)
	{
		await Task.Delay(0);
		//throw new NotImplementedException();
	}

	public async Task LogJsonText(object obj, string key, string sessionKey = "")
	{
		await Task.Delay(0);
		//throw new NotImplementedException();
	}

	public async Task<string> LogCriticalError(string key, string url, string sessionId, string request, string contents)
	{
		await Task.Delay(0);
		//throw new NotImplementedException();
		return string.Empty;
	}

	protected async Task LogXmlObject(object obj, LogXmls key, string sessionKey = "", IXmlRedactor? redactor = null)
	{
		await Task.Delay(0);
		//throw new NotImplementedException();
	}
}