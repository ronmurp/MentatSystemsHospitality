using System.ComponentModel.DataAnnotations;
using Msh.HotelCache.Models.Discounts;

namespace Msh.HotelCache.Models.Hotels;

/// <summary>
/// This is a test model to experiment with forms and validation
/// </summary>
public class TestModel
{
	[Required(ErrorMessage = ConstHotel.Vem.CodeRequired)]
	[Length(3, 5, ErrorMessage = ConstHotel.Vem.CodeLength35)]
	[Display(Name = "Model Code")]
	public string Code { get; set; } = string.Empty;

	[Required]
	[Length(3, 50, ErrorMessage = ConstHotel.Vem.NameLength350)]
	public string? Name { get; set; } // = string.Empty;

	//[Required]
	//[EmailAddress]
	//[DataType(DataType.EmailAddress)]
	//public string Email { get; set; } = string.Empty;

	//[Required]
	//[Length(ConstHotel.Vem.PasswordMin, ConstHotel.Vem.PasswordMax, ErrorMessage = ConstHotel.Vem.CodeLength35)]
	//[DataType(DataType.Password)]
	//public string Password { get; set; } = string.Empty;

	//[Required]
	//public string Language { get; set; } = string.Empty;

	
	[Length(0, 500)]
	public string? Notes { get; set; } = string.Empty;

	//public List<string> Hotels { get; set; } = [];

	//public BookingStatus BookingStatus { get; set; } = BookingStatus.Any;
}