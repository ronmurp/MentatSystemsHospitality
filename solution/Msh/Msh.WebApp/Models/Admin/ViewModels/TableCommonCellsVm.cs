namespace Msh.WebApp.Models.Admin.ViewModels;

public class TableCommonCellsVm
{
	public string HotelCode { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;

	public string EditController { get; set; } = "Hotels";
	public string EditAction { get; set; } = string.Empty;
	public int Index { get; set; }

	public int Count { get; set; }

	/// <summary>
	/// These flags default to true because we usually want them all.
	/// Can deselect any we don't want.
	/// </summary>
	public bool IncludeMove { get; set; } = true;
	public bool IncludeEdit { get; set; } = true;
	public bool IncludeCopy { get; set; } = true;
	public bool IncludeCheckbox { get; set; } = true;
}