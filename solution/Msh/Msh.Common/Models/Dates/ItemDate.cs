namespace Msh.Common.Models.Dates;

public class ItemDate
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public bool Enabled { get; set; } = true;
    public string Notes { get; set; } = string.Empty;

}