namespace Msh.WebApp.Areas.Admin.Models
{
	public class AdminSelectListVm
	{
		public List<AdminSelectItem> SelectList { get; set; } = [];
	}

	public class AdminSelectItem
	{
		public string Value { get; set; } = string.Empty;
		public string Text { get; set; } = string.Empty;
	}
}
