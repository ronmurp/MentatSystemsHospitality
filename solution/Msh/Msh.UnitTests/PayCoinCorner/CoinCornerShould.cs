using Msh.Common.Models.Configuration;
using Msh.Pay.CoinCorner.Models;
using NUnit.Framework;
using System.Text.Json;
using Msh.TestSupport;

namespace Msh.UnitTests.PayCoinCorner;

[TestFixture]
public class CoinCornerShould
{
    [Test]
    [TestCase("Test Notes 1")]
    [TestCase("Test Notes 2")]
    public void AddConfigurationOnce(string notes)
    {
        var sut = TestConfigUtilities.GetRepository();

        var coinCornerConfig = new CoinCornerConfig() { UserId = "USER"};

        var json = JsonSerializer.Serialize(coinCornerConfig);

        var config = new Config
        {
            ConfigType = "TestConfig",
            Content = json,
            Notes = notes

        };
       
        Assert.That(true, Is.True, "Phase 1");

        sut.AddConfig(config);
        
    }

    [Test]
    public void SaveExistingConfiguration()
    {
        var sut = TestConfigUtilities.GetRepository();

        var config = sut.GetConfig("TestConfig");

        var json = config.Content;

        var ccConfig = JsonSerializer.Deserialize<CoinCornerConfig>(json);

        Assert.That(ccConfig.UserId, Is.EqualTo("USER"));
    }

    [Test]
    public void RemoveExistingConfiguration()
    {
        var sut = TestConfigUtilities.GetRepository();

        sut.RemoveConfig("TestConfig");
    }

}