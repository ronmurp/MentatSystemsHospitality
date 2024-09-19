using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Provides a subset of security service methods for user logins
/// </summary>
public interface IOperaSecurityService
{
	/// <summary>
	/// Creates a userLogin+password for a profileId
	/// </summary>
	(OwsUser owsUser, OwsResult owsResult) CreateUserRequest(OwsUser user);

	/// <summary>
	/// Authenticates a user for login
	/// </summary>
	(OwsUser owsUser, OwsResult owsResult) AuthenticateUserRequest(OwsUser user);

	/// <summary>
	/// Updates a password. This is not a recovery call.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="oldPassword"></param>
	/// <param name="newPassword"></param>
	/// <returns></returns>
	(OwsUser owsUser, OwsResult owsResult) UpdatePassword(OwsUser user, string oldPassword, string newPassword);

	(OwsUser owsUser, OwsResult owsResult) ResetPasswordRequest(OwsUser user);

	(List<OwsQuestion> questions, OwsResult owsResult) FetchQuestionListRequest(OwsUser user);
        
	(OwsUser owsUser, OwsResult owsResult) UpdateQuestionRequest(OwsUser user);
       
}