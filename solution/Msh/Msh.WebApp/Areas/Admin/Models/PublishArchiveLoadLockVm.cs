namespace Msh.WebApp.Areas.Admin.Models;

public class PublishArchiveLoadLockVm
{
	public string OnclickPublish { get; set; } = "window.mshMethods.publishHotels()";
	public string OnclickArchive { get; set; } = "window.mshMethods.archiveHotels()";

	public string OnclickLoad { get; set; } = "window.mshMethods.loadHotels()";

	public string OnclickLock { get; set; } = "window.mshMethods.lockHotels()";

	public string Controller { get; set; } = "Hotels";

	public string Action { get; set; } = "Index";
}