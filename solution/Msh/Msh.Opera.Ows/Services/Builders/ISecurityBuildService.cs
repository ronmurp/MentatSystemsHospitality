using System.Xml.Linq;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Builders;

public interface ISecurityBuildService
{
	XElement AuthenticateUserRequest(OwsUser user, OwsConfig config);
	XElement CreateUserRequest(OwsUser user, OwsConfig config);
	XElement FetchQuestionListRequest(OwsUser user, OwsConfig config);
	XElement ResetPasswordRequest(OwsUser user, OwsConfig config);

	XElement UpdateQuestionRequest(OwsUser user, OwsConfig config);
	XElement UpdatePasswordRequest(OwsUser user, string oldPassword, string newPassword, OwsConfig config);
}