using System.Text.Json.Serialization;

namespace Msh.Common.Models.Dates;

/// <summary>
/// Temporary replacement for BaseDatedItem while refactoring 'Dirty' properties
/// </summary>
public abstract class BaseDatedItem
{
    public List<ItemDate> ItemDates { get; set; } = new List<ItemDate>();

    public string Code { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public bool EnabledAll { get; set; }
    public abstract bool IsValid(DateTime arrive, DateTime depart);

    [JsonIgnore]
    public bool HasDates => ItemDates.Count > 0;
}