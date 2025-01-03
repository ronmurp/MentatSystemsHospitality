﻿namespace Msh.WebApp.Areas.Admin.Models;

/// <summary>
/// An input class that can be used for API calls
/// </summary>
public class ApiInput
{
	public string Code { get; set; } = string.Empty;
	public string HotelCode { get; set; } = string.Empty;

	public string UniqueCode { get; set; } = string.Empty;

	public string NewCode { get; set; } = string.Empty;
	public string NewHotelCode { get; set; } = string.Empty;
	
	// When copy needs a date qualifier - as for RatePlans
	public string NewDate { get; set; } = string.Empty;

	public List<string> CodeList { get; set; } = [];

	public int Direction { get; set; }

	public bool IsTrue { get; set; }
}