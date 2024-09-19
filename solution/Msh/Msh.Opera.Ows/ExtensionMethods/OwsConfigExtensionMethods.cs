using Microsoft.VisualBasic;
using Msh.Common.Exceptions;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.ExtensionMethods;

public static class OwsConfigExtensionMethods
{

	public static string InformationUrl(this OwsConfig c) => $"{c.BaseUrl}Information";
	public static string AvailabilityUrl(this OwsConfig c) => $"{c.BaseUrl}Availability";
	public static string ReservationUrl(this OwsConfig c) => $"{c.BaseUrl}Reservation";

	public static string ResvAdvancedUrl(this OwsConfig c) => $"{c.BaseUrl}ResvAdvanced";
	public static string SecurityUrl(this OwsConfig c) => $"{c.BaseUrl}Security";
	public static string NameUrl(this OwsConfig c) => $"{c.BaseUrl}Name";
	public static bool Applies(this CriticalErrorTrigger cet, CriticalErrorCall callType) =>
		cet.CallTypes.Any(ct => ct == callType || ct == CriticalErrorCall.Any);
}