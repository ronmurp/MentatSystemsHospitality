namespace Msh.WebApp.Areas.Admin.Models;

/// <summary>
/// Determines how the 
/// </summary>
public class PublishArchiveLoadLockVm : SaveReturnButtonsVm
{
	
	public string OnclickPublish { get; set; } = "window.mshMethods.publishData()";
	public string OnclickArchive { get; set; } = "window.mshMethods.archiveData()";

	public string OnclickLoad { get; set; } = "window.mshMethods.loadData()";

	public string OnclickLock { get; set; } = "window.mshMethods.lockData()";
}

public class SaveReturnButtonsVm
{/// <summary>
	/// Determines the save button use:
	/// - "" - Save button not included - typically for lists
	/// - "Submit" - Use the form submit action
	/// - "#xxx" - Use a listener ( $(#xxx).on('click', ...)
	/// - "window.mshMethods..."
	/// </summary>
	public string SaveOperation { get; set; } = string.Empty;

	public string ReturnUrl { get; set; } = string.Empty;

	public string ReturnText { get; set; } = ConstReturnText.ReturnToDash;
}