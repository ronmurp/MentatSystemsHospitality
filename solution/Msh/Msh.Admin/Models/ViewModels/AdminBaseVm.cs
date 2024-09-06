namespace Msh.Admin.Models.ViewModels;

public class AdminBaseVm
{
}

public class AdminMenuItem
{
	public string Name { get; set; } = string.Empty;
	public string Controller { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string Active { get; set; } = string.Empty;
    
}