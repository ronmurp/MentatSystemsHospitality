namespace Msh.WebApp.Models.Admin.ViewModels
{
	public class CodeCheckListVm
	{
		public string HotelCode { get; set; } = string.Empty;

		public string Code { get; set; } = string.Empty;

		public List<CodeCheckListRow> List { get; set; } = [];
	}

	public class CodeCheckListRow
	{
		public string GroupCode { get; set; } = string.Empty;

		public string Code { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public bool Selected { get; set; }
	}
}
