using NUnit.Framework;

namespace Msh.UnitTests.Basic;

[TestFixture]
public class DateTimeShould
{
    [Test]
    public void ShowUtc()
    {
        var dtu = DateTime.UtcNow;
        var dto = DateTimeOffset.UtcNow;
        var dt = DateTime.Now;
        

        Console.WriteLine($"{dto:O} - {dto.ToLocalTime():O}");
        Console.WriteLine($"{dt:O} - {dt.ToLocalTime():O}");
        Console.WriteLine($"{dtu:O} - {dtu.ToLocalTime():O}");
    }
}