namespace Msh.Admin.Models;

/// <summary>
/// An admin user, their admin name, and their permissions
/// </summary>
public class AdminUserPermissions
{
	/// <summary>
	/// The user's email, from the AspNetUsers table
	/// </summary>
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// A user's name entered in the admin data here, not in AspNetUsers
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The user's email has been confirmed, in the AspNetUsers table
	/// </summary>
	public bool Confirmed { get; set; }

	public List<AdminPermission> Permissions { get; set; } = new List<AdminPermission>();
}

public class AdminPermission
{
	public AdminActivity Activity { get; set; } = AdminActivity.ViewHotels;
	public string Name { get; set; } = string.Empty;
}

public enum AdminActivity
{
	ViewHotels, AddHotel, EditHotel, DeleteHotel,
	ViewTestModels, AddTestModel, EditTesModel, DeleteTestModel
}