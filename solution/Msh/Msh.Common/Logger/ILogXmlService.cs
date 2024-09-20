using System.Xml.Linq;
using Msh.Common.Models;

namespace Msh.Common.Logger;

public interface ILogXmlService
{
	Task LogXml(object obj, string key, string sessionKey = "", IXmlRedactor? redactor = null);

	Task LogXmlText(string xml, string key, string sessionKey = "", IXmlRedactor? redactor = null);

	Task LogJsonText(object obj, string key, string sessionKey = "");

	Task<string> LogCriticalError(string key, string url, string sessionId, string request, string contents);
}

public interface IXmlRedactor
{
	void Redact(XDocument xdoc);

	void RedactNoStrip(XDocument xdoc);

	bool Enabled { get; set; }
}

public interface ILogCodesService
{
	List<LogCodesItem> GetLogCodes();

	List<LogCodesItem> LoadLogCodes(string appDataPath, string userId = "");

	bool SaveLogCodes(string appDataPath, List<LogCodesItem> list, string userId);

	string Lookup(string logCode);
}