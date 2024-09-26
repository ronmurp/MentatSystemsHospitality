using System.Reflection;
using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportDiscounts
{

	[Test]
	public async Task GetConnectionString()
	{
		Console.WriteLine(TestConfigUtilities.GetConnectionString());

		var repo = TestConfigUtilities.GetRepository();

		var config = await repo.GetConfigAsync("Hotel");
	}
    [Test]
    public void ImportDiscountGroups()
    {
        var config = new List<DiscountGroup>
        {
            new()
            {
                Name = "Festive",
                Description = "Discounts for the festive season."
            },
            new()
            {
                Name = "FIT-ABC",
                Description = "Discounts for the FIT - Agent Code ABC."
            },
            new()
            {
                Name = "FIT-DEF",
                Description = "Discounts for the FIT - Agent Code DEF."
            },
            new()
            {
                Name = "Nights",
                Description = "Discounts for the a number of Nights."
            },
            new()
            {
                Name = "R2R",
                Description = "Discounts for Reason to Return."
            },
            new()
            {
                Name = "Together",
                Description = "Discounts for Together."
            },
            new()
            {
                Name = "TravelZoo",
                Description = "Discounts for TravelZoo."
            }
        };

        // TestConfigUtilities.SaveConfig(ConstHotel.Cache.DiscountGroups, config);

    }

    [Test]
    [TestCase("LWH")]
    public async Task ImportDiscountsByHotel(string hotelCode)
    {
	    var dir = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\DiscountCodes";
	    var files = Directory.GetFiles(dir);

	    var bigList = new List<DiscountCode>();

		foreach (var f in files)
		{
			var xdoc = XDocument.Load(f);
			var list = xdoc.Descendants("DiscountCode")
				.Select(c => new DiscountCode
				{
					Code = c.ValueA("code"),
					Enabled = c.ValueA(false, "enabled"),
					Selectable = c.ValueA(false, "selectable"),
					MinRooms = c.ValueE(0, "MinRooms"),
					MinNights = c.ValueE(0, "MinNights"),

					MaxRooms = c.ValueE(0, "MaxRooms"),
					MaxNights = c.ValueE(0, "MaxNights"),

					DiscountType = c.ValueE("DiscountType").Get<DiscountTypes>(),
					Discount = c.ValueE(0, "Discount"),

					// Must default true for backward compatibility
					// Included in codes (along with rate plans) that are NOT full pre-pay
					FullPrepay = c.ValueE(true, "FullPrepay"),

					MinTotal = c.ValueE(1M, "MinTotal"),
					MinTotalMessage = c.ValueE("MinTotalMessage"),
					LimitedDiscountMessage = c.ValueE("LimitedDiscountMessage"),

					// Just the name of an email list
					EmailValidationList = c.ValueE("EmailValidationList"),

					Description = c.ValueE("Description"),
					ShortDescription = c.ValueE("ShortDescription"),
					Details = c.ValueE("Details"),
					DiscountWarning = c.ValueE("DiscountWarning"),

					RequiresCopyPaste = c.ValueE(false, "RequiresCopyPaste"),

					// OfferDates = GetOfferDates(c),

					OneTime = c.Descendants("OneTime")
				          .Select(o => new DiscountOneTime
				          {
					          Mode = o.ValueE("None", "Mode").Get<OneTimeMode>(),
					          RequiresReservationId = o.ValueE(false, "RequiresReservationId"),
					          RequiresResvId = o.ValueE(false, "RequiresResvId"),
					          RequiresProfileId = o.ValueE(false, "RequiresProfileId"),
					          RequiresEmail = o.ValueE(false, "RequiresEmail"),
					          RequiresLogin = o.ValueE(false, "RequiresLogin"),
					          RequiresPastDepart = o.ValueE(false, "RequiresPastDepart"),
					          HashMethod = o.ValueE("None", "oneTimeHash").Get<OneTimeHashVersion>(),

					          BookingStatus = o.ValueE("Any", "BookingStatus")
						                          ?.Get<BookingStatus>()
					                          ?? BookingStatus.Any,

					          ExpireCount = o.ValueE(0, "ExpireCount"),

					          ExpireCountMode = o.Descendant("ExpireCount")
						                            ?.ValueA("Weeks", "mode")
						                            ?.Get<ExpireCountMode>()
					                            ?? ExpireCountMode.None,

					          AllowRollover = o.ValueE(false, "AllowRollover"),
					          UpdateBooker = o.ValueE(false, "UpdateBooker")
				          })
				          .SingleOrDefault()
			          ?? new DiscountOneTime(),

					//DiscountErrors = LoadXml(c.Descendants("ErrorText")),
					DiscountErrors = LoadXml(c.Descendants("ErrorText")),

				}).ToList();

			bigList.AddRange(list);

		}

		await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.Discounts}-{hotelCode}", bigList);
	}

    public List<DiscountError> LoadXml(IEnumerable<XElement> el)
    {
	    var config = el
		    .Select(c => new DiscountErrorsOld()
		    {
			    InvalidCode = c.ValueE("InvalidCode"),

			    RequiresCopyPasteCode = c.ValueE("RequiresCopyPasteCode"),

			    NotApplied = c.ValueE("NotApplied"),

			    WrongHotel = c.ValueE("WrongHotel"),
			    SelectHotel = c.ValueE("SelectHotel"),
			    SelectAnyHotel = c.ValueE("SelectAnyHotel"),
			    SelectAnotherHotel = c.ValueE("SelectAnotherHotel"),

			    NotAvailableDates = c.ValueE("NotAvailableDates"),
			    InvalidBookDates = c.ValueE("InvalidBookDates"),

			    InvalidRooms = c.ValueE("InvalidRooms"),
			    InvalidNights = c.ValueE("InvalidNights"),


			    DisabledByRatePlan = c.ValueE("DisabledByRatePlan"),

			    EnterEmail = c.ValueE("EnterEmail"),

			    OneTimeInvalid = c.ValueE("OneTimeInvalid"),
			    OneTimeExpired = c.ValueE("OneTimeExpired"),
			    OneTimeUsed = c.ValueE("OneTimeUsed"),

			    InvalidBooking = c.ValueE("InvalidBooking"),
			    InvalidBookingStatus = c.ValueE("InvalidBookingStatus"),
			    InvalidDepartDate = c.ValueE("InvalidDepartDate"),

			    RoomLimit = c.ValueE("RoomLimit"),

			    SearchError = c.ValueE("SearchError")

		    }).SingleOrDefault() ?? new DiscountErrorsOld();

	    var list = new List<DiscountError>();
	    PropertyInfo[] properties = typeof(DiscountErrorsOld).GetProperties();
	    foreach (PropertyInfo p in properties)
	    {
		    var value = (p.GetValue(config) as string) ?? string.Empty;
		    if (!string.IsNullOrEmpty(value))
		    {
			    var de = new DiscountError
			    {
					ErrorType = p.Name.Get<DiscountErrorType>(),
					Text = value
			    };
			    list.Add(de);
			}
		    
	    }

		return list;
    }
}