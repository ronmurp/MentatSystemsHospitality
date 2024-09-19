namespace Msh.Opera.Ows.Models;

/// <summary>
/// A user object passed to Name and Security service builders
/// </summary>
public class OwsUser : OwsBaseSession
{
	/// <summary>
	/// Email address
	/// </summary>
	public string?  LoginName { get; set; }
	public string?  Password { get; set; }

	/// <summary>
	/// The user profile that's associated with the email address - there may be many.
	/// </summary>
	public string?  ProfileId { get; set; }

	/// <summary>
	/// When resetting the password, a question ID and answer must be supplied
	/// </summary>
	public string?  QuestionId { get; set; }
	public string?  QuestionAnswer { get; set; }

	public string?  Title { get; set; }
	public string?  FirstName { get; set; }
	public string?  LastName { get; set; }

	public string?  Email { get; set; }
}