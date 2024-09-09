using Msh.Admin.Models.ViewModels;

namespace Msh.Admin.Services;

public interface IAdminVmService
{
	List<AdminMenuItem> SetActiveItem(string currentItem, List<AdminMenuItem> items);
}
public class AdminVmService : IAdminVmService
{
	public List<AdminMenuItem> SetActiveItem(string currentItem, List<AdminMenuItem> items)
	{
		foreach (var item in items.Where(item => item.Name == currentItem))
		{
			item.Active = "active";
			break;
		}

		return items;
	}
}