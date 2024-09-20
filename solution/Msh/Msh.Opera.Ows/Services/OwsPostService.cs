using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;
using Msh.Common.Exceptions;
using Msh.Common.Logger;
using Msh.Common.Services;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Config;
using Msh.Opera.Ows.Services.Helpers;

namespace Msh.Opera.Ows.Services;

public class OwsPostService(
	ILogXmlService logXmlService,
	IOwsConfigService owsConfigService,
	ICriticalErrorService criticalErrorService)
	: IOwsPostService
{
	public async Task<(XDocument xdoc, string contents, OwsResult owsResult)> PostAsync(StringBuilder sb, string url, string sessionId = "")
	{
		var retryCount = owsConfigService.OwsConfig.RetryCount;

		var count = 0;
		var contents = "";
		OwsResult httpOwsResult = null;

		while(true)
		{
			if (++count > retryCount)
			{
				throw RetryError($"Retry count {count} greater than max {retryCount}, SessionId {sessionId}", "PostAsync");
			}

			try
			{
				using (var client = new HttpClient())
				{
					var httpContent = new StringContent(sb.ToString(), Encoding.UTF8, "text/xml");

					client.DefaultRequestHeaders.Add("User-Agent", "WBS");

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

					var response = await client.PostAsync(new Uri(url), httpContent);

					httpOwsResult = null;

					if (response.StatusCode != HttpStatusCode.OK)
					{
						httpOwsResult = OwsResultHelper.HttpResult(response);
					}

					contents = await response.Content.ReadAsStringAsync();

					var (hasError, xdoc) = CheckForCriticalErrors(url, sessionId, sb.ToString(), contents, count);

					if (hasError) continue;

					return (xdoc, contents, httpOwsResult);

				}

			}
			catch (Exception ex)
			{
				//WbsLogger.Error("CRITICAL-ERROR", ex, $"Retry count {count}, SessionId {sessionId}");
				await logXmlService.LogXml(contents, LogXmls.OwsCriticalErrorPostAsync, $"{sessionId}");
				if (count > retryCount)
					throw ex;
			}
		}

	}

	private (bool hasError, XDocument xdoc) CheckForCriticalErrors(string url, string sessionId, string request, string contents, int count)
	{
		var list = owsConfigService.OwsConfig.CriticalErrorTriggers;
		var retryCount = owsConfigService.OwsConfig.RetryCount;

		var result = criticalErrorService.CheckForCriticalError(list, contents, count, retryCount, LogCodes.OwsCriticalError, url, string.Empty);

		if (result.foundCriticalError)
		{
			var filename = logXmlService.LogCriticalError("CriticalError", url, sessionId, request, contents);
			var obj = new
			{
				CriticalErrorCode = result.errorCode,
				SessionId = sessionId,
				SoapFile = filename
			};
			//WbsLogger.Error(LogCodes.OwsCriticalError, obj, $"Retry count {count}, SessionId {sessionId}. SOAP file {filename}");
			return (result.canRetry, null);
		}
		//foreach (var t in list)
		//{
		//    if (contents.Contains(t.Trigger))
		//    {
		//        var filename = _logXmlService.LogCriticalError("CriticalError", url, sessionId, request, contents);
		//        WbsLogger.Error(LogCodes.CriticalError, new Exception($"{t.Code}"), $"Retry count {count}, SessionId {sessionId}. SOAP file {filename}");
                    
		//        return (true, null);
		//    }
		//}

		try
		{
			var xdoc = XDocument.Parse(contents);

			return (false, xdoc);
		}
		catch (Exception ex)
		{
			var filename = logXmlService.LogCriticalError("CriticalError", url, sessionId, request, contents);
			//WbsLogger.Error(LogCodes.OwsCriticalError, ex, $"Error parsing XML. Retry count {count}, SessionId {sessionId}. SOAP file {filename}");

			return (true, null);
		}
	}

	private LibException RetryError(string message, string method)
	{
		var ex = new LibException(message, method)
		{
			ErrorCodePrefix = "WBS",
			ErrorCodeSuffix = "RETRY",
			ErrorType = "R",
			AdditionalText = ""
		};

		return ex;
	}
}