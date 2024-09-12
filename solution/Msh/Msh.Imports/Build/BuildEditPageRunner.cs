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
	private void BuildPage(Type classType, string publishName)
	{
		var service = new EditPageBuildService();

		var output = service.BuildEditPage(classType, publishName);

		Console.WriteLine(output.ToString());
	}
}