using Msh.Common.Models.Dates;

namespace Msh.HotelCache.Models.Specials;

public class Special
{
	
	public string Code { get; set; } = string.Empty;
	public bool Enabled { get; set; }
	public string Text { get; set; } = string.Empty;
	public SpecialItemType ItemType { get; set; } = SpecialItemType.CheckBox;
	public ItemDateType DateType { get; set; } = ItemDateType.Prohibit;
	public string SelectedText { get; set; } = string.Empty;
	public string ShortText { get; set; } = string.Empty;
	public bool IsValued { get; set; } = false;
	public bool SingleLine { get; set; } = false;

	public List<SelectOption> Options { get; set; } = [];

	public List<ItemDate> ItemDates { get; set; } = [];
	public DisablingSet DisablingSet { get; set; } = new DisablingSet();
	public string WarningText { get; set; } = string.Empty;

}