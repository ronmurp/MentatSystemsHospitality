namespace Msh.Common.Models.Dates;

/// <summary>
/// Whether a date pair inhibits or allows
/// Allow: IsValid if tested dates are in this date range
/// Prohibit: !IsValid if tested dates are in this range
/// </summary>
public enum ItemDateType
{
    Allow, Prohibit
}