using System.Text;
using System.Xml.Linq;
using Msh.Common.Logger;
using Msh.Opera.Ows.Cache;
using Msh.Opera.Ows.ExtensionMethods;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Base;
using Msh.Opera.Ows.Services.Builders;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Provides a subset of security service methods for user logins
/// </summary>
public class OperaSecurityService(
	IOwsCacheService owsCacheService,
	IOwsPostService owsPostService,
	ISecurityBuildService securityBuildService,
	ILogXmlService logXmlService)
	: OperaBaseService(logXmlService, owsCacheService, owsPostService), IOperaSecurityService
{
	public async Task<(OwsUser owsUser, OwsResult owsResult)> AuthenticateUserRequestAsync(OwsUser user)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = securityBuildService.AuthenticateUserRequest(user, config);

		var sb = new StringBuilder(xElement.ToString());

		// _logXmlService.LogXmlText(sb.ToString(), "AuthenticateUserReq", user.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.SecurityUrl());

		//_logXmlService.LogXmlText(contents, "AuthenticateUserRes", user.SessionKey);

		var decode = DecodeAuthenticateUserResponse(xdoc, contents);

		return (decode.user, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsUser owsUser, OwsResult owsResult)> CreateUserRequestAsync(OwsUser user)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = securityBuildService.CreateUserRequest(user, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), LogXmls.OwsCreateUserReq, user.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.SecurityUrl());

		await _logXmlService.LogXmlText(contents, LogXmls.OwsCreateUserRes, user.SessionKey);

		var decode = DecodeCreateUserResponse(xdoc, contents);

		return (decode.user, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsUser owsUser, OwsResult owsResult)> UpdatePasswordAsync(OwsUser user, string oldPassword, string newPassword)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = securityBuildService.UpdatePasswordRequest(user, oldPassword, newPassword, config);

		var sb = new StringBuilder(xElement.ToString());

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.SecurityUrl());

		var decode = DecodeUpdatePasswordResponse(user, xdoc, contents);

		// return the user because on success nothing is added
		return (user, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsUser owsUser, OwsResult owsResult)> ResetPasswordRequestAsync(OwsUser user)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = securityBuildService.ResetPasswordRequest(user, config);

		var sb = new StringBuilder(xElement.ToString());

		//await _logXmlService.LogXmlText(sb.ToString(), "ResetPasswordReq", user.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.SecurityUrl());

		//await _logXmlService.LogXmlText(contents, "ResetPasswordRes", user.SessionKey);

		var decode = DecodeResetPasswordResponse(xdoc, contents);

		return (decode.user, decode.owsResult ?? owsResult);
	}

	public async Task<(List<OwsQuestion> questions, OwsResult owsResult)> FetchQuestionListRequestAsync(OwsUser user)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = securityBuildService.FetchQuestionListRequest(user, config);

		var sb = new StringBuilder(xElement.ToString());

		//await _logXmlService.LogXmlText(sb.ToString(), "FetchQuestionListReq", user.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.SecurityUrl());

		//await _logXmlService.LogXmlText(contents, "FetchQuestionListRes", user.SessionKey);

		var decode = DecodeFetchQuestionListResponse(xdoc, contents);

		return (decode.questions, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsUser owsUser, OwsResult owsResult)> UpdateQuestionRequestAsync(OwsUser user)
	{
		var config = await _owsCacheService.GetOwsConfig();

		var xElement = securityBuildService.UpdateQuestionRequest(user, config);

		var sb = new StringBuilder(xElement.ToString());

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.SecurityUrl());

		var decode = DecodeUpdateQuestionResponse(xdoc, contents);

		// return the user because on success nothing is added
		return (user, decode.owsResult ?? owsResult);
	}


	private (OwsUser user, OwsResult owsResult) DecodeUpdatePasswordResponse(OwsUser user, XDocument xdocInput, string contents)
	{
		const string mainElement = "UpdatePasswordResponse";
		const string methodName = "DecodeUpdatePasswordResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsUser, owsResult);

		var owsUser = xdoc.Descendants(mainElement)
			.Select(u => new OwsUser
			{
				ProfileId = LinqXmlExtensionMethods.Descendant(u, "NameID").ValueE()
			}).SingleOrDefault();

		var owsResultDecode = CheckForNoData(owsUser, methodName);

		if (owsResultDecode != null)
			return (null as OwsUser, owsResultDecode);

		return (owsUser, new OwsResult(true));
	}

	protected (OwsUser user, OwsResult owsResult) DecodeAuthenticateUserResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "AuthenticateUserResponse";
		const string methodName = "DecodeAuthenticateUserResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsUser, owsResult);

		var owsUser = xdoc.Descendants(mainElement)
			.Select(u => new OwsUser
			{
				ProfileId = LinqXmlExtensionMethods.Descendant(u, "NameID").ValueE()
			}).SingleOrDefault();

		var owsResultDecode = CheckForNoData(owsUser, methodName);

		if (owsResultDecode != null)
			return (null as OwsUser, owsResultDecode);

		return (owsUser, new OwsResult(true));
	}

	protected (OwsUser user, OwsResult owsResult) DecodeCreateUserResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "CreateUserResponse";
		const string methodName = "DecodeCreateUserResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsUser, owsResult);


		return (null, new OwsResult(true));
	}

	protected (List<OwsQuestion> questions, OwsResult owsResult) DecodeFetchQuestionListResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "FetchQuestionListResponse";
		const string methodName = "DecodeFetchQuestionListResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as List<OwsQuestion>, owsResult);

		var list = xdoc.Descendants(mainElement).Descendants("Questions")
			.Select(u => new OwsQuestion
			{
				Id = u.ValueA("questionId"),
				Question = u.ValueE()
			}).ToList();

		var owsResultDecode = CheckForNoData(list, methodName);

		if (owsResultDecode != null)
			return (null as List<OwsQuestion>, owsResultDecode);

		return (list, new OwsResult(true));
	}

	protected (OwsUser user, OwsResult owsResult) DecodeResetPasswordResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "ResetPasswordResponse";
		const string methodName = "DecodeResetPasswordResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsUser, owsResult);


		return (null, new OwsResult(true));
	}

	protected (OwsUser user, OwsResult owsResult) DecodeUpdateQuestionResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "UpdateQuestionResponse";
		const string methodName = "DecodeUpdateQuestionResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null as OwsUser, owsResult);

		return (null, new OwsResult(true));
	}

}