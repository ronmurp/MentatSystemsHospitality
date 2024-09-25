namespace Msh.WebApp.Areas.Admin.Models;

public class PublishArchiveLoadLockVm
{
	public bool IncludeSave { get; set; }
	public string OnclickPublish { get; set; } = "window.mshMethods.publishHotels()";
	public string OnclickArchive { get; set; } = "window.mshMethods.archiveHotels()";

	public string OnclickLoad { get; set; } = "window.mshMethods.loadHotels()";

	public string OnclickLock { get; set; } = "window.mshMethods.lockHotels()";

	public string ReturnController { get; set; } = "Hotels";

	public string ReturnAction { get; set; } = "Index";

	public string ReturnText { get; set; } = "Return to list";

	public string SaveController { get; set; } = "Hotels";

	public string SaveAction { get; set; } = "Index";
}