namespace Msh.Admin.Models.ViewModels;

public class AdminBaseVm
{
}

/// <summary>
/// An admin menu item provides information for an asp anchor tag
/// </summary>
public class AdminMenuItem
{
	public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The name of the Controller
    /// </summary>
	public string Controller { get; set; } = string.Empty;

    /// <summary>
    /// The name of the action on the controller
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// The text to display in hte anchor
    /// </summary>
    public string Text { get; set; } = string.Empty;


    public string Active { get; set; } = string.Empty;
    
}