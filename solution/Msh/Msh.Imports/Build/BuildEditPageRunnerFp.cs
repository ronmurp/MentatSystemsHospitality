using Msh.Opera.Ows.Models;
using Msh.Pay.FreedomPay.Models.Configuration;
using NUnit.Framework;

namespace Msh.Imports.Build;

[TestFixture]
[Explicit]
public class BuildEditPageRunnerFp
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
		_helper.BuildPage(typeof(FpConfig), "FreedomPay", "Index");
	}

	

}