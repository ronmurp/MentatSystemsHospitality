using Msh.Common.Exceptions;

namespace Msh.Common.Services;

/// <summary>
/// Service to check for critical errors
/// </summary>
public interface ICriticalErrorService
{
	(bool foundCriticalError, bool canRetry, string errorCode) CheckForCriticalError(List<CriticalErrorTrigger> criticalErrorTriggers,
		string contents, int count, int retryCount, string logCode, string url, string message);
}