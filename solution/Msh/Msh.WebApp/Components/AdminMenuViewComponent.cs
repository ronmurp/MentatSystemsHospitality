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
				case AdminConst.MenuAdmin:
					list = AdminMenu();
					break;

				case AdminConst.MenuUsers:
					list = AdminMenuUsers();
					break;

				case AdminConst.MenuHotels:
					list = AdminMenuHotels();
					break;

				case AdminConst.MenuDev:
					list = AdminMenuDev();
					break;

				case AdminConst.MenuCoinCorner:
					list = AdminMenuCoinCorner();
					break;

				case AdminConst.MenuAdministrator:
					list = AdminMenuAdministrator();
					break;

				case AdminConst.MenuOws:
					list = AdminMenuOws();
					break;
				case AdminConst.MenuLog:
					list = AdminMenuLog();
					break;

				case AdminConst.MenuFp:
					list = AdminMenuFp();
					break;

				default:
					return [];
			}

			return list;
		}

		private AdminMenuItem AdminRootItem(string name)
		{
			switch (name)
			{
				case AdminConst.MenuAdmin:
					return new AdminMenuItem { Name = "AdminIndex", Controller = "Home", Action = "Index", Text = "Admin" };
				case AdminConst.MenuUsers:
					return new AdminMenuItem { Name = "UsersIndex", Controller = "Users", Action = "Index", Text = "Users" };
				case AdminConst.MenuDev:
					return new AdminMenuItem { Name = "DevIndex", Controller = "Dev", Action = "Index", Text = "Dev" };
				case AdminConst.MenuFp:
					return new AdminMenuItem { Name = "FpIndex", Controller = "Fp", Action = "Index", Text = "FreedomPay" };
				case AdminConst.MenuCoinCorner:
					return new AdminMenuItem { Name = "CcIndex", Controller = "CoinCorner", Action = "Index", Text = "Coin Corner" };
				case AdminConst.MenuAdministrator:
					return new AdminMenuItem { Name = "Administrator", Controller = "Administrator", Action = "Index", Text = "Administrator" };
				case AdminConst.MenuHotels:
					return new AdminMenuItem { Name = "HotelsIndex", Controller = "Hotels", Action = "Index", Text = "Hotels" };
				case AdminConst.MenuOws:
					return new AdminMenuItem { Name = "OwsIndex", Controller = "Ows", Action = "Index", Text = "OWS" };
				case AdminConst.MenuLog:
					return new AdminMenuItem { Name = "OwsIndex", Controller = "Loggers", Action = "Index", Text = "Loggers" };
				default:
					return new AdminMenuItem { Name = "AdminIndex", Controller = "Admin", Action = "Index", Text = "Admin" };
			}
		}

		private List<AdminMenuItem> AdminMenu() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuUsers),
			AdminRootItem(AdminConst.MenuHotels),
			AdminRootItem(AdminConst.MenuDev),
			AdminRootItem(AdminConst.MenuFp),
			AdminRootItem(AdminConst.MenuCoinCorner),
			AdminRootItem(AdminConst.MenuAdministrator),
			AdminRootItem(AdminConst.MenuOws),
			AdminRootItem(AdminConst.MenuLog)
		];

		private List<AdminMenuItem> AdminMenuHotels() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuHotels),
			new AdminMenuItem { Name = "HotelsList", Controller = "Hotels", Action = "HotelsList", Text = "Hotels" },
			new AdminMenuItem { Name = "RoomTypesList", Controller = "Hotels", Action = "RoomTypesList", Text = "Room Types" },
			new AdminMenuItem { Name = "RatePlansList", Controller = "RatePlans", Action = "RatePlansList", Text = "Rate Plans" },
			new AdminMenuItem { Name = "RatePlansTextList", Controller = "RatePlansTexts", Action = "RatePlansTextList", Text = "Rate Plans Text" },
			new AdminMenuItem { Name = "RatePlanSortList", Controller = "RatePlansSort", Action = "RatePlanSortList", Text = "Rate Plan Sort" },
			new AdminMenuItem { Name = "ExtrasList", Controller = "Extras", Action = "ExtrasList", Text = "Extras" },
			new AdminMenuItem { Name = "SpecialsList", Controller = "Specials", Action = "SpecialsList", Text = "Specials" },
			new AdminMenuItem { Name = "DiscountsList", Controller = "Discounts", Action = "DiscountsList", Text = "Discounts" },
			new AdminMenuItem { Name = "TestModelList", Controller = "Hotels", Action = "TestModelList", Text = "Test Models" }
		];
		private List<AdminMenuItem> AdminMenuDev() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuDev),
			new AdminMenuItem { Name = "DevScriptsTest", Controller = "Dev", Action = "ScriptsTest", Text = "Scripts Tests" },
			new AdminMenuItem { Name = "DevConfigStates", Controller = "Dev", Action = "ConfigStateList", Text = "Config States" }
		];
		private List<AdminMenuItem> AdminMenuCoinCorner() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuCoinCorner),
			new AdminMenuItem { Name = "CcConfig", Controller = "CoinCorner", Action = "Config", Text = "CC Config" },
			new AdminMenuItem { Name = "CcConfig", Controller = "CoinCorner", Action = "Global", Text = "CC Global" }
		];

		private List<AdminMenuItem> AdminMenuAdministrator() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuAdministrator),
			new AdminMenuItem { Name = "UserList", Controller = "Administrator", Action = "UserList", Text = "User List" }
		];
		private List<AdminMenuItem> AdminMenuUsers() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuUsers),
			new AdminMenuItem { Name = "UserList", Controller = "Users", Action = "UserList", Text = "User List" }
		];

		private List<AdminMenuItem> AdminMenuOws() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuOws),
			new AdminMenuItem { Name = "OwsConfigEdit", Controller = "Ows", Action = "OwsConfigEdit", Text = "OWS Config" },
            new AdminMenuItem { Name = "OwsConfigEditMaps", Controller = "Ows", Action = "OwsConfigEditMaps", Text = "Schema Maps" },
            new AdminMenuItem { Name = "OwsConfigEditTriggers", Controller = "Ows", Action = "OwsConfigEditTriggers", Text = "Crit. Error Triggers" },
            new AdminMenuItem { Name = "OwsConfigEditMaps", Controller = "Ows", Action = "OwsRawAvailability", Text = "Raw Availability" },
        ];

		private List<AdminMenuItem> AdminMenuLog() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuLog),
			new AdminMenuItem { Name = "LogConfigEdit", Controller = "Loggers", Action = "LogXmlConfigEdit", Text = "Log Config" },
			
		];

		private List<AdminMenuItem> AdminMenuFp() =>
		[
			AdminRootItem(AdminConst.MenuAdmin),
			AdminRootItem(AdminConst.MenuFp),
			new AdminMenuItem { Name = "FpConfigEdit", Controller = "Fp", Action = "FpConfigEdit", Text = "FP Config" },
			new AdminMenuItem { Name = "FpErrorBankList", Controller = "Fp", Action = "FpErrorBankList", Text = "FP Error Bank" },

		];
	}
}
