using System.Text;
using System.Xml.Linq;
using Msh.Common.Logger;
using Msh.Common.Models.BaseModels;
using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.ExtensionMethods;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Base;
using Msh.Opera.Ows.Services.Builders;
using Msh.Opera.Ows.Services.Config;

namespace Msh.Opera.Ows.Services;

public class OperaNameService(
	IOwsConfigService owsConfigService,
	IOwsPostService owsPostService,
	INameBuildService nameBuildService,
	ILogXmlService logXmlService)
	: OperaBaseService(owsConfigService.OwsConfig, logXmlService, owsConfigService, owsPostService), IOperaNameService
{
	public async Task<(OwsProfile owsProfile, OwsResult owsResult)> FetchProfile(OwsBaseSession reqData, string profileId)
	{
		var config = _config;

		var xElement = nameBuildService.FetchProfileRequest(reqData, profileId, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchProfileReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl());

		await _logXmlService.LogXmlText(contents, "FetchProfileRes", reqData.SessionKey);

		var decode = DecodeProfileResponse(xdoc, contents);

		return (decode.owsProfile, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsProfile owsProfile, OwsResult owsResult)> FetchProfileAsync(OwsBaseSession reqData, string profileId)
	{
		var config = _config;

		var xElement = nameBuildService.FetchProfileRequest(reqData, profileId, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchProfileReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl(), reqData.SessionKey);

		await _logXmlService.LogXmlText(contents, "FetchProfileRes", reqData.SessionKey);

		var decode = DecodeProfileResponse(xdoc, contents);

		return (decode.owsProfile, decode.owsResult ?? owsResult);
	}

	public async Task<(List<OwsProfile> owsProfiles, OwsResult owsResult)> NameLookupRequestAsync(OwsBaseSession reqData, string email)
	{
		var config = _config;

		var xElement = nameBuildService.NameLookupRequestByEmail(reqData, email, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchProfileReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl());

		await _logXmlService.LogXmlText(contents, "FetchProfileRes", reqData.SessionKey);

		var decode = DecodeNameLookupResponse(xdoc, contents);

		return (decode.owsProfiles, decode.owsResult ?? owsResult);
	}

	public async Task<(List<OwsProfile> owsProfiles, OwsResult owsResult)> NameLookupRequestByNameAsync(OwsBaseSession reqData, string name, string nameType)
	{
		var config = _config;

		var xElement = nameBuildService.NameLookupRequestByName(reqData, name, nameType, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchProfileReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl());

		await _logXmlService.LogXmlText(contents, "FetchProfileRes", reqData.SessionKey);

		var decode = DecodeNameLookupResponse(xdoc, contents);

		return (decode.owsProfiles, decode.owsResult ?? owsResult);
	}

	public async Task<(List<OwsProfile> owsProfiles, OwsResult owsResult)> NameLookupRequestByPersonAsync(OwsBaseSession reqData, string firstName, string lastName, string email)
	{
		var config = _config;

		var xElement = nameBuildService.NameLookupRequestByPerson(reqData, firstName, lastName, email, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchProfileReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl());

		await _logXmlService.LogXmlText(contents, "FetchProfileRes", reqData.SessionKey);

		var decode = DecodeNameLookupResponse(xdoc, contents);

		return (decode.owsProfiles, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsProfile owsProfile, OwsResult owsResult)> FetchNameAsync(OwsBaseSession reqData, string profileId)
	{
		var config = _config;

		var xElement = nameBuildService.FetchNameRequest(reqData, profileId, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchNameReq", reqData.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl());

		await _logXmlService.LogXmlText(contents, "FetchNameRes", reqData.SessionKey);

		var decode = DecodeNameResponse(xdoc, contents);

		return (decode.owsProfile, decode.owsResult ?? owsResult);
	}

	public async Task<(OwsProfile owsProfile, OwsResult owsResult)> RegisterNameAsync(OwsUser user)
	{
		var config = _config;

		var xElement = nameBuildService.RegisterNameRequest(user, config);

		var sb = new StringBuilder(xElement.ToString());

		await _logXmlService.LogXmlText(sb.ToString(), "FetchNameReq", user.SessionKey);

		var (xdoc, contents, owsResult) = await PostAsync(sb, config.NameUrl());

		await _logXmlService.LogXmlText(contents, "FetchNameRes", user.SessionKey);

		var decode = await DecodeRegisterNameResponse(xdoc, contents, user);

		return (decode.owsProfile, decode.owsResult ?? owsResult);
	}

	private async Task<(OwsProfile owsProfile, OwsResult owsResult)> DecodeRegisterNameResponse(XDocument xdocInput, string contents, OwsUser user)
	{

		const string mainElement = "RegisterNameResponse";
		const string methodName = "DecodeRegisterNameResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null, owsResult)!;

		var prof = xdoc.Descendants("IDPair")
			.Select(p => new
			{
				ProfileId = p.ValueA("operaId"),
			}).FirstOrDefault();

		if (prof == null)
		{
			var nullResult = CheckForNoData(prof, "DecodeRegisterNameResponse");
			return (null, nullResult)!;
		}

		var result = await FetchProfile(user, prof.ProfileId);

		return result;
	}

	protected (OwsProfile owsProfile, OwsResult owsResult) DecodeProfileResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "FetchProfileResponse";
		const string methodName = "DecodeProfileResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null, owsResult)!;

		var prof = xdoc.Descendants("ProfileDetails")
			.Select(p => new
			{
				Active = p.ValueA(false, "active"),
				ID = p.Descendants("UniqueID")
					.Select(r => new
					{
						ProfileId = r.ValueE(),
						IdType = r.ValueA("type")

					}).FirstOrDefault()
			}).SingleOrDefault();


		var customer = xdoc.Descendants("ProfileDetails").Descendants("Customer")
			.Select(c => new
			{
				CustomerType = c.ValueA("profileType"), // GUEST/CONTACT/
				Name = c.Descendants("PersonName")
					.Select(n => new
					{
						FirstName = n.ValueE("firstName"),
						LastName = n.ValueE("lastName"),
						Title = n.ValueE("nameTitle")
					}).SingleOrDefault(),
			}).SingleOrDefault();

		var addresses = xdoc.Descendants("ProfileDetails").Descendants("Addresses").Descendants("NameAddress")
			.Select(a => new
			{
				AddressType = a.ValueA("addressType"),
				OperaId = a.ValueA("operaId"),
				Primary = a.ValueA(false, "primary"),
				UpdatedDate = a.ValueA(DateTime.MinValue, "updateDate"),
				AddressLines = a.Descendants("AddressLine")
					.Select(al => new
					{
						Value = al.ValueE()
					}).ToList(),
				City = a.Descendant("cityName").ValueE(),
				StateProvince = a.Descendant("stateProv").ValueE(),
				CountryCode = a.Descendant("countryCode").ValueE(),
				PostalCode = a.Descendant("postalCode").ValueE()

			}).ToList();

		var phones = xdoc.Descendants("ProfileDetails").Descendants("Phones").Descendants("NamePhone")
			.Select(ph => new
			{
				PhoneType = ph.ValueA("phoneType"),
				PhoneRole = ph.ValueA("phoneRole"),
				OperaId = ph.ValueA("operaId"),
				Primary = ph.ValueA(false, "primary"),
				UpdatedDate = ph.ValueA(DateTime.MinValue, "updateDate"),
				Number = ph.ValueE("PhoneNumber")
			}).ToList();

		var emails = xdoc.Descendants("ProfileDetails").Descendants("EMails").Descendants("NameEmail")
			.Select(ne => new
			{
				OperaId = ne.ValueA("operaId"),
				EmailType = ne.ValueA("emailType"),
				Primary = ne.ValueA(false, "primary"),
				UpdatedDate = ne.ValueA(DateTime.MinValue, "updateDate"),
				Email = ne.ValueE()
			}).ToList();

		var company = xdoc.Descendants("CompanyName")
			.Select(co => new
			{
				Name = co.ValueE()

			}).FirstOrDefault();

		//_logXmlService.LogJsonText(profile, "FetchProfileRes");

		var owsResultDecode = CheckForNoData(prof, methodName);

		if (owsResultDecode != null)
			return (null, owsResultDecode);

		var address = addresses.FirstOrDefault();
		var email = emails.FirstOrDefault();
		var phone = phones.FirstOrDefault();

		var owsProfile = new OwsProfile
		{
			ProfileId = prof.ID.ProfileId ?? string.Empty,
			CustomerType = customer?.CustomerType ?? string.Empty,
			CompanyName = company?.Name ?? string.Empty,
			Title = customer?.Name?.Title ?? string.Empty,
			FirstName = customer?.Name?.FirstName ?? string.Empty,
			LastName = customer?.Name?.LastName ?? string.Empty,
			Address = new BaseAddress
			{
				OperaId = address ?.OperaId ?? string.Empty,
				AddressLine1 = address?.AddressLines.Count > 0 ? (address?.AddressLines[0]?.Value ?? string.Empty) : string.Empty,
				AddressLine2 = address?.AddressLines.Count > 1 ? address?.AddressLines[1]?.Value ?? string.Empty : string.Empty,
				City = address?.City ?? string.Empty,
				StateProvince = address?.StateProvince ?? string.Empty,
				CountryCode = address?.CountryCode ?? string.Empty,
				PostalCode = address?.PostalCode ?? string.Empty
			},
			Email = email?.Email ?? string.Empty,
          
		};
		foreach (var p in phones)
		{
			owsProfile.Phones.Add(new OwsProfilePhone
			{
				Number = p?.Number ?? string.Empty,
				Primary = p?.Primary ?? true,
				PhoneType = p?.PhoneType ?? "HOME",
				PhoneRole = p?.PhoneRole ?? string.Empty
			});
		}


		return (owsProfile, new OwsResult(true));
	}

        
	protected (OwsProfile owsProfile, OwsResult owsResult) DecodeNameResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "FetchNameResponse";
		const string methodName = "DecodeNameResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null, owsResult);

		var profile = xdoc.Descendants("PersonName")
			.Select(p => new
			{
				Active = p.ValueA(false, "active"),
				FirstName = p.ValueE("firstName"),
				LastName = p.ValueE("lastName"),
				Title = p.ValueE("nameTitle")
			}).SingleOrDefault();

		//_logXmlService.LogJsonText(profile, "FetchProfileRes");

		var owsResultDecode = CheckForNoData(profile, methodName);

		if (owsResultDecode != null)
			return (null, owsResultDecode);

		var owsProfile = new OwsProfile
		{
			Title = profile?.Title ?? string.Empty,
			FirstName = profile?.FirstName ?? string.Empty,
			LastName = profile?.LastName ?? string.Empty,
		};
            
		return (owsProfile, new OwsResult(true));
	}

	protected (List<OwsProfile> owsProfiles, OwsResult owsResult) DecodeNameLookupResponse(XDocument xdocInput, string contents)
	{
		const string mainElement = "NameLookupResponse";
		const string methodName = "DecodeNameLookupResponse";

		var (xdoc, owsResult) = ParseAndCheckForFail(xdocInput, contents, mainElement, methodName);

		if (owsResult != null)
			return (null, owsResult);

		var profiles = xdoc.Descendants("Profiles").Descendants("Profile")
			.Select(p => new OwsProfile
			{
				ProfileId = p.Descendant("ProfileIDs", "UniqueID").ValueE(),
				FirstName = p.Descendant("Customer", "PersonName").ValueE("firstName"),
				LastName = p.Descendant("Customer", "PersonName").ValueE("lastName"),
				Title = p.Descendant("Customer", "PersonName").ValueE("nameTitle"),
				Email = p.Descendant("EMails", "NameEmail").ValueE()
			}).ToList();

		//_logXmlService.LogJsonText(profile, "FetchProfileRes");

		var owsResultDecode = CheckForNoData(profiles, methodName);

		if (owsResultDecode != null)
			return (null, owsResultDecode);

          
		return (profiles, new OwsResult(true));
	}

}