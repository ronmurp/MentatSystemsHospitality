namespace Msh.Common.Exceptions;

public enum CriticalErrorType
{
	Critical, // Critical, but can retry - it might be due to comms
	NoRepeat // We know this and don't want to keep trying
}