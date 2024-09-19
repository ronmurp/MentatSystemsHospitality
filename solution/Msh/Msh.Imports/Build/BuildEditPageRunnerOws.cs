using Msh.Opera.Ows.Models;
using NUnit.Framework;

namespace Msh.Imports.Build;

[TestFixture]
[Explicit]
public class BuildEditPageRunnerOws
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
		_helper.BuildPage(typeof(OwsConfig), "OWS Config");
	}

	

}