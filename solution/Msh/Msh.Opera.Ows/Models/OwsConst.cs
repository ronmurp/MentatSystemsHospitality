namespace Msh.Opera.Ows.Models;

public static class OwsConst
{
	public static class Cache
	{
		public const string OwsConfig = "OwsConfig";
		public const string OwsErrorList = "OwsErrorList";
	}
	public static class OgHeader
	{
		public const string SoapBody = "{{Soap.Body}}";

		public const string TimeStampCreated = "{{TimeStamp.Created}}";
		public const string TimeStampExpires = "{{TimeStamp.Expires}}";
		public const string ElhUserId = "{{ElhUserId}}";
		public const string Password = "{{Password}}";
		public const string TransactionId = "{{TransactionId}}";
		public const string HotelCode = "{{HotelCode}}";
		public const string ChainCode = "{{ChainCode}}";
	}

	public class Information
	{
		public const string ChainCode = "{{ChainCode}}";
	}
	public static class LovQuery2
	{
		public const string RequestId = "{{LovQuery2.RequestId}}";
		public const string BusinessDate = "BUSINESSDATE";
		public const string CountryCodes = "COUNTRYCODES";
	}

	public static class Avail
	{
		public const string Arrive = "{{Arrive}}";
		public const string Depart = "{{Depart}}";
		public const string Adults = "{{Adults}}";
		public const string Children = "{{Children}}";
		public const string NumberOfRooms = "{{NumberOfRooms}}";
		public const string IncludeRestricted = "{{IncludeRestricted}}";

		public const string RatePlanCandidate = "{{RatePlanCandidate}}";

		public const string PromotionCode = "{{PromotionCode}}";
		public const string CompanyType = "{{CompanyType}}";
		public const string CompanyCode = "{{CompanyCode}}";
		public const string RoomTypeCode = "{{RoomTypeCode}}";
		public const string RatePlanCode = "{{RatePlanCode}}";

		public class CompanyTypes
		{
			public const string Company = "COMPANY";
			public const string Corporate = "CORPORATE";
			public const string TravelAgent = "TRAVEL_AGENT";
		}
	}


	public static class Res
	{
		public const string GuaranteeText = "{{GuaranteeText}}";
		public const string ResGuestRPHIndexes = "{{ResGuestRPHIndexes}}";
		public const string RoomStayComments = "{{RoomStayComments}}";
		public const string Title = "{{Title}}";
		public const string FirstName = "{{FirstName}}";
		public const string LastName = "{{LastName}}";
		public const string GuestAddressPhones = "{{GuestAddressPhones}}";
		public const string AddressLine1 = "{{AddressLine1}}";
		public const string AddressLine2 = "{{AddressLine2}}";
		public const string CityName = "{{CityName}}";
		public const string StateProvince = "{{StateProvince}}";
		public const string CountryCode = "{{CountryCode}}";
		public const string PostalCode = "{{PostalCode}}";
		public const string Telephone = "{{Telephone}}";
		public const string Email = "{{Email}}";
		public const string CommentText = "{{CommentText}}";
		public const string Index = "{{Index}}";
		public const string ReservationAction = "{{ReservationAction}}";

		public const string UniqueIdList = "{{UniqueIdList}}";
		public const string UniqueIdType = "{{UniqueIdType}}";
		public const string UniqueIdSource = "{{UniqueIdSource}}";
		public const string UniqueIdValue = "{{UniqueIdValue}}";
		public const string SpecialRequests = "{{SpecialRequests}}";
		public const string Packages = "{{Packages}}";
		public const string ReservationId = "{{ReservationId}}";
		public const string Quantity = "{{Quantity}}";
		public const string ProductCode = "{{ProductCode}}";
	}
}