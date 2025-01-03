﻿namespace Msh.WebApp.Areas.Admin.Models;

/// <summary>
/// Determines how the 
/// </summary>
public class PublishArchiveLoadLockVm : SaveReturnButtonsVm
{
	
	public string OnclickPublish { get; set; } = "window.mshMethods.publishData(true)";
	public string OnclickArchive { get; set; } = "window.mshMethods.archiveData(true)";

	public string OnclickLoad { get; set; } = "window.mshMethods.loadData(true)";

	public string OnClickDelete { get; set; } = "window.mshMethods.deleteData(true)";

	public string OnclickLock { get; set; } = "window.mshMethods.lockData(true)";
	public bool UseHotel { get; set; }
	
}

public enum SaveReturnTypes
{
	NoSave,
	Submit,
	EventListener,
	OnclickMethod
}

public class SaveReturnButtonsVm
{
	public SaveReturnTypes ButtonType { get; set; } = SaveReturnTypes.NoSave;

	/// <summary>
	/// Determines the save button use:
	/// - "" - Save button not included - typically for lists
	/// - "Submit" - Use the form submit action
	/// - "#xxx" - Use a listener ( $(#xxx).on('click', ...)
	/// - "window.mshMethods..."
	/// </summary>
	public string SaveOperation { get; set; } = string.Empty;

	public string ReturnUrl { get; set; } = string.Empty;

	public string ReturnText { get; set; } = ConstReturnText.ReturnToDash;


	public bool ShowImportButton { get; set; }

	public string? OnclickImport { get; set; } = "window.mshMethods.importData(true)";
}