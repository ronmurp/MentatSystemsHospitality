using Msh.Common.Models.Dates;

namespace Msh.Common.ExtensionMethods;

public static class ItemDateExtensionMethods
{
	/// <summary>
	/// The hotel is disabled
	/// </summary>
	/// <param name="hotelDateItem"></param>
	/// <param name="arrive"></param>
	/// <param name="depart"></param>
	/// <returns>Returns true if disabled</returns>
	/// <remarks>
	/// 
	/// |------|                |----| OK
	///         |- IsDisabled -|
	///       |----| |---|  |-----| disable
	///       |-------------------| disable
	/// </remarks>
	public static bool IsDisabled(this ItemDate hotelDateItem, DateOnly arrive, DateOnly depart) =>
		hotelDateItem.Enabled && !(depart <= hotelDateItem.FromDate || arrive > hotelDateItem.ToDate);

	/// <summary>
	/// True if the date is within the date range
	/// </summary>
	/// <param name="hotelDateItem"></param>
	/// <param name="date"></param>
	/// <returns></returns>
	public static bool IsDisabled(this ItemDate hotelDateItem, DateOnly date) =>
		hotelDateItem.Enabled && date >= hotelDateItem.FromDate && date <= hotelDateItem.ToDate;

	/// <summary>
	/// Cannot book the hotel if the 
	/// </summary>
	/// <param name="hotelDateItem"></param>
	/// <param name="now"></param>
	/// <returns></returns>
	public static bool CannotBook(this ItemDate hotelDateItem, DateOnly now) =>
		hotelDateItem.Enabled && now >= hotelDateItem.FromDate && now <= hotelDateItem.ToDate;

}