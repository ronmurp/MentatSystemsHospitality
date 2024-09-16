namespace Msh.WebApp.Models.Admin.ViewModels;

public class TableCommonCellsVm
{
	public string HotelCode { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;

	public string EditAction { get; set; } = string.Empty;
	public int Index { get; set; }

	public int Count { get; set; }
}