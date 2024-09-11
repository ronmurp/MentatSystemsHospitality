namespace Msh.Common.Models.Dates;

public class ItemDate
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool Enabled { get; set; } = true;
    public string Notes { get; set; } = string.Empty;

}