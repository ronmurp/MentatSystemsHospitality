using Msh.WebApp.Models;
using NUnit.Framework;

namespace Msh.UnitTests.Basic;

[TestFixture]
public class ApplicationConfigurationShould
{
    /// <summary>
    /// Test that the ApplicationConfiguration returns appropriate strings
    /// </summary>
    /// <param name="key"></param>
    [Test]
    [TestCase("ConnectionStrings:DefaultConnection")]
    public void ReturnCorrectAppSettingsValue(string key)
    {
        var value = ApplicationConfiguration.GetSetting(key);

        Console.WriteLine(value);
    }
}