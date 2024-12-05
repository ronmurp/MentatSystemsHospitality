using Msh.HotelCache.Models;
using Msh.TestSupport;
using NUnit.Framework;
using System.Xml.Linq;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportRatePlans
{
	[Test]
	[TestCase("LWH")]
	[TestCase("LHH")]
	[TestCase("WBH")]
	public async Task ImportRatePlanLwh(string hotelCode)
	{
		var fitEnabled = false;

		var filename = @$"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\RatePlans\RatePlans-{hotelCode}.xml";

		var xdoc = XDocument.Load(filename);

		var ratePlans = xdoc.Descendants("RatePlanCodes")
			.Select(x => new RatePlanCodesList()
			{
				GroupCode = x.ValueA("groupDefault"),
				RoomRatePlans = x.Descendants("RatePlanCode")
					.Select(el => new RoomRatePlan()
					{
						IsPromo = el.ValueA(false, "isPromo"),
						RatePlanCode = el.ValueA("code"),
						BaseRate = el.ValueA("baseRate"),
						Group = el.ValueA("group"),
						//GroupSortOrder = groupSorts.SingleOrDefault(v => v.Id == groupCode)?.SortOrder ?? 0,
						MarketSegment = el.ValueA("marketSegment"),
						SourceCode = el.ValueA("sourcecode"),

						IsBaseRate = el.ValueA(false, "isBaseRate"),
						IsDbb = el.ValueA(false, "isDBB"),
						IsOffer = el.ValueA(false, "isOffer"),
						FitEnabled = el.ValueA(false, "fitEnabled") || fitEnabled,
						MinNights = el.ValueA(0, "minNights"),
						MaxNights = el.ValueA(0, "maxNights"),

						DbbText = el.ValueA("dbbText"),
						FullPrePay = el.ValueA(true, "fullPrePay"),
						DepositAmount = el.ValueA(0, "depositAmount"),
						DepositDays = el.ValueA(0, "depositDays"),
						DepositBalanceMessage = el.ValueE("DepositBalanceMessage"),

						Description = el.ValueE("description"),
						ShortDescription = el.ValueE("shortDescription"),

						// There's no title for common & discounted rates, so use shortDescription
						Title = el.ValueE("shortDescription"), //useShort ? el.ValueE("shortDescription") : el.ValueE("title"),
						SubTitle = el.ValueE("subtitle"),

						//ConfirmationText = _commonTextService.GetExplicitOrCommonText(_commonTexts, el, "ConfirmationText"),

						DefaultDescription = el.ValueE("defaultDescription"),
						UnavailableOffer = el.ValueE("UnavailableOffer"),
						StayFrom = GetDate(el, "stayFrom"),
						StayTo = GetDate(el, "stayTo"),
						//OfferDates = GetOfferDates(el).FirstOrDefault() ?? new OfferDate(),

						DisableDiscount = el.ValueE(false, "DisableDiscount"),

						WithDiscounts = (el.Descendants("WithDiscounts")
								.FirstOrDefault()
								?.Value ?? string.Empty)
							.SplitCommaTrimUpper().ToList(),

						//BookingComments = _commonTextService.GetCommonTexts(el, "comment")


					}).ToList()
			}).ToList();

		var list = new List<RoomRatePlan>();

		foreach (var rpg in ratePlans)
		{
			foreach (var rp in rpg.RoomRatePlans)
			{
				if (string.IsNullOrEmpty(rp.Group))
				{
					rp.Group = rpg.GroupCode;
					
				}
				list.Add(rp);
			}
		}
			

		await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.RatePlans}-{hotelCode}", list);
	}


	private DateOnly GetDate(XElement el, string s)
	{
		var d = el.Descendants("Date")
			.Select(x => new
			{
				StayFrom = x.ValueA(new DateOnly(), "stayFrom"),
				StayTo = x.ValueA(new DateOnly(), "stayTo")
			}).SingleOrDefault();

		if (d == null)
			return new DateOnly();

		return s == "stayFrom" ? d.StayFrom : d.StayTo;

	}

	[Test]
	[TestCase("LWH")]
	[TestCase("LHH")]
	[TestCase("WBH")]
	public async Task ImportRatePlanText(string hotelCode)
	{
		var fitEnabled = false;

		var filename = @$"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\RatePlans\RatePlans-CommonTexts.xml";

		var xdoc = XDocument.Load(filename);

		var ratePlansText = xdoc.Descendants("Text")
			.Select(x => new RatePlanText()
			{
				
						Id = x.ValueA("id"),
						Text = x.ValueE()

			}).ToList();

		await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.RatePlansText}-{hotelCode}", ratePlansText);
	}

	
	public class RatePlanCodesList
	{
		public string GroupCode { get; set; }
		public List<RoomRatePlan> RoomRatePlans { get; set; }
	}

	public class HotelRoomTypes
	{
		public string HotelCode { get; set; }

		public bool UseRoomTypeGroups { get; set; }

		public List<RoomType> RoomTypeList { get; set; }

	}
}