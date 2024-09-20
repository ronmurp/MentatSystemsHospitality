using Msh.Loggers.XmlLogger;
using NUnit.Framework;

namespace Msh.Imports.Build;

[TestFixture]
[Explicit]
public class BuildEditPageRunnerLog
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
		_helper.BuildPage(typeof(LogXmlConfig), "Log XML Config");
	}

	

}