namespace Msh.Common.Exceptions;

public enum CriticalErrorCall
{
	Any,        // Default
	PcRes,      // Premier Core reservation
	PcBill,     // Premier Core Bill,
	PcClient,    // Premier Core Client,
	PcResWarn,

	GpLookup,
	GpRedeem
}