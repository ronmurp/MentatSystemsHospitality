namespace Msh.WebApp.Areas.Admin.Models.Ows
{
	
	/// <summary>
	/// Admin vm for requesting services.
	/// </summary>
	/// <remarks>
	/// Used in:
	/// - AdminAvailabilityController.Availability
	/// </remarks>
	public class RawAvailabilityReq
	{
		public string? OwsSource { get; set; }

		public string? HotelCode { get; set; }
		public DateTime Arrive { get; set; }
		public DateTime Depart { get; set; }

		public int Nights { get; set; } = 1;

		public string? DataType { get; set; }

		public int Adults { get; set; }
		public int Children { get; set; }

		public string? Sort { get; set; }
		public string? IncludePaths { get; set; }
		public string? ResId { get; set; }

		public string? QualifyingIdType { get; set; } = string.Empty;

		public string? QualifyingIdValue { get; set; } = string.Empty;

		public List<string> List { get; set; } = [];

	}
}
