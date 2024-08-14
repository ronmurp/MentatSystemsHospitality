using Msh.Common.Models.Configuration;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;
using Msh.Pay.CoinCorner.Models;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportDiscounts
{
    [Test]
    public void ImportDiscountGroups()
    {
        var config = new List<DiscountGroup>
        {
            new()
            {
                Name = "Festive",
                Description = "Discounts for the festive season."
            },
            new()
            {
                Name = "FIT-ABC",
                Description = "Discounts for the FIT - Agent Code ABC."
            },
            new()
            {
                Name = "FIT-DEF",
                Description = "Discounts for the FIT - Agent Code DEF."
            },
            new()
            {
                Name = "Nights",
                Description = "Discounts for the a number of Nights."
            },
            new()
            {
                Name = "R2R",
                Description = "Discounts for Reason to Return."
            },
            new()
            {
                Name = "Together",
                Description = "Discounts for Together."
            },
            new()
            {
                Name = "TravelZoo",
                Description = "Discounts for TravelZoo."
            }
        };

        TestConfigUtilities.SaveConfig(ConstHotel.Cache.DiscountGroups, config);
    }
}