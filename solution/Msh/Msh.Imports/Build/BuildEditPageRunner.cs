using NUnit.Framework;

namespace Msh.Imports.Build;

[TestFixture]
[Explicit]
public class BuildEditPageRunner
{
	private BuildEditPageRunnerHelper _helper;
	[SetUp]
	public void Setup()
	{
		_helper = new BuildEditPageRunnerHelper();
	}

	[Test]
	public void BuildHotelEditPage()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.Hotels.Hotel), "Hotel List");
	}

	[Test]
	public void BuildRoomTypeEditPage()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.RoomTypes.RoomType), "Room Type List");
	}

	[Test]
	public void BuildExtraEditPage()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.Extras.Extra), "Extras List");
	}

	[Test]
	public void BuildRatePlanEditPage()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.RatePlans.RoomRatePlan), "Rate Plan List");
	}

	[Test]
	public void BuildSpecialEditPage()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.Specials.Special), "Specials List");
	}

	[Test]
	public void BuildDiscountsEditPage()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.Discounts.DiscountCode), "Discounts List");
	}

	[Test]
	public void BuildDiscountsErrors()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.Discounts.DiscountErrors), "Discounts Errors");
	}

	[Test]
	public void BuildDiscountsOneTime()
	{
		_helper.BuildPage(typeof(Msh.HotelCache.Models.Discounts.DiscountOneTime), "Discounts One Time");
	}
	

}