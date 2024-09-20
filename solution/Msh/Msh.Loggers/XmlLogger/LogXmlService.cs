using Msh.Common.Logger;

namespace Msh.Loggers.XmlLogger;

public class LogXmlService : ILogXmlService
{
	public async Task LogXml(object obj, string key, string sessionKey = "", IXmlRedactor? redactor = null)
	{
		if (obj is string xml)
			await LogXmlText(xml, key, sessionKey, redactor);
		else
			await LogXmlObject(obj, key, sessionKey, redactor);
	}

	public Task LogXmlText(string xml, string key, string sessionKey = "", IXmlRedactor? redactor = null)
	{
		throw new NotImplementedException();
	}

	public Task LogJsonText(object obj, string key, string sessionKey = "")
	{
		throw new NotImplementedException();
	}

	public Task<string> LogCriticalError(string key, string url, string sessionId, string request, string contents)
	{
		throw new NotImplementedException();
	}
}