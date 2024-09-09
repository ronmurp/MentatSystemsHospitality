using Microsoft.AspNetCore.Mvc;
using Msh.Admin.Models.Const;
using Msh.Admin.Models.ViewModels;

namespace Msh.WebApp.Components
{
	public class AdminMenuViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(string menuName, string currentItem)
		{
			await Task.Delay(0);

			// get the menu by name
			var list = GetMenu(menuName, currentItem);

			foreach (var item in list)
			{
				if (item.Name == currentItem)
				{
					item.Active = "active";
					break;
				}
			}
			return View(list);
		}
		
		private List<AdminMenuItem> GetMenu(string menuName, string currentItem)
		{
			List<AdminMenuItem> list;

			switch (menuName)
			{
				case AdminConst.MenuHotels:
					list = AdminMenuHotels();
					break;

				case AdminConst.MenuDev:
					list = AdminMenuDev();
					break;

				case AdminConst.MenuCoinCorner:
					list = AdminMenuCoinCorner();
					break;

				default:
					return [];
			}

			return list;
		}

		private List<AdminMenuItem> AdminMenuHotels() =>
		[
			new AdminMenuItem { Name = "HotelsIndex", Controller = "Hotels", Action = "Index", Text = "Hotels Home" },
			new AdminMenuItem { Name = "HotelList", Controller = "Hotels", Action = "HotelList", Text = "Hotels" },
			new AdminMenuItem { Name = "RoomTypeList", Controller = "Hotels", Action = "RoomTypeList", Text = "Room Types" },
			new AdminMenuItem { Name = "TestModelList", Controller = "Hotels", Action = "TestModelList", Text = "Test Models" }
		];
		private List<AdminMenuItem> AdminMenuDev() =>
		[
			new AdminMenuItem { Name = "DevIndex", Controller = "Dev", Action = "Index", Text = "Dev Home" },
			new AdminMenuItem { Name = "DevScriptsTest", Controller = "Dev", Action = "ScriptsTest", Text = "Scripts Tests" }
		];
		private List<AdminMenuItem> AdminMenuCoinCorner() =>
		[
			new AdminMenuItem { Name = "CcIndex", Controller = "CoinCorner", Action = "Index", Text = "Coin Corner Home" },
			new AdminMenuItem { Name = "CcConfig", Controller = "CoinCorner", Action = "Config", Text = "CC Config" },
			new AdminMenuItem { Name = "CcConfig", Controller = "CoinCorner", Action = "Global", Text = "CC Global" }
		];
	}
}
