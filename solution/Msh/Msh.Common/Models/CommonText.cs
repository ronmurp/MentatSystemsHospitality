namespace Msh.Common.Models;

/// <summary>
/// A common text element
/// </summary>
public class CommonText : BaseCommonText
{
    public DateTime StartDate { get; set; } = DateTime.MinValue;
    public DateTime EndDate { get; set; } = DateTime.MaxValue;
}