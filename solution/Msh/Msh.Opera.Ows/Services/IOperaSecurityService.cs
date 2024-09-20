using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Provides a subset of security service methods for user logins
/// </summary>
public interface IOperaSecurityService
{
	/// <summary>
	/// Authenticates a user for login
	/// </summary>
	Task<(OwsUser owsUser, OwsResult owsResult)> AuthenticateUserRequestAsync(OwsUser user);

	/// <summary>
	/// Creates a userLogin+password for a profileId
	/// </summary>
	Task<(OwsUser owsUser, OwsResult owsResult)> CreateUserRequestAsync(OwsUser user);


	/// <summary>
	/// Updates a password. This is not a recovery call.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="oldPassword"></param>
	/// <param name="newPassword"></param>
	/// <returns></returns>
	Task<(OwsUser owsUser, OwsResult owsResult)> UpdatePasswordAsync(OwsUser user, string oldPassword, string newPassword);

	Task<(OwsUser owsUser, OwsResult owsResult)> ResetPasswordRequestAsync(OwsUser user);

	Task<(List<OwsQuestion> questions, OwsResult owsResult)> FetchQuestionListRequestAsync(OwsUser user);

	Task<(OwsUser owsUser, OwsResult owsResult)> UpdateQuestionRequestAsync(OwsUser user);
       
}