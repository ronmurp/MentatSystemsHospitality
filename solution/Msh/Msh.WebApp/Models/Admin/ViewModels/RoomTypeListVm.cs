﻿using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class HotelListVm
{
	public string HotelCode { get; set; } = string.Empty;

	public List<Hotel> Hotels { get; set; } = [];

	public string ErrorMessage { get; set; } = string.Empty;
}
public class RoomTypeListVm : HotelListVm
{
	public List<RoomType> RoomTypes { get; set; } = [];

}