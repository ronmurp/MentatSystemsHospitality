using NUnit.Framework;

namespace Msh.Imports.Build;

[TestFixture]
[Explicit]
public class BuildEditPageRunner
{
	[Test]
	public void BuildHotelEditPage()
	{
		BuildPage(typeof(Msh.HotelCache.Models.Hotels.Hotel), "Hotel List");
	}

	[Test]
	public void BuildRoomTypeEditPage()
	{
		BuildPage(typeof(Msh.HotelCache.Models.RoomTypes.RoomType), "Room Type List");
	}

	[Test]
	public void BuildExtraEditPage()
	{
		BuildPage(typeof(Msh.HotelCache.Models.Extras.Extra), "Extras List");
	}

	[Test]
	public void BuildRatePlanEditPage()
	{
		BuildPage(typeof(Msh.HotelCache.Models.RatePlans.RoomRatePlan), "Rate Plan List");
	}

	[Test]
	public void BuildSpecialEditPage()
	{
		BuildPage(typeof(Msh.HotelCache.Models.Specials.Special), "Specials List");
	}
	private void BuildPage(Type classType, string publishName)
	{
		var service = new EditPageBuildService();

		var output = service.BuildEditPage(classType, publishName);

		Console.WriteLine(output.ToString());
	}
}