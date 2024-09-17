namespace Msh.WebApp.Models.Admin.ViewModels
{
	public class SpecialRoomTypesVm
	{
		public string HotelCode { get; set; } = string.Empty;

		public string Code { get; set; } = string.Empty;

		public List<SpecialRoomTypeRow> List { get; set; } = [];
	}

	public class SpecialRoomTypeRow
	{
		public string GroupCode { get; set; } = string.Empty;

		public string Code { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public bool Selected { get; set; }
	}
}
