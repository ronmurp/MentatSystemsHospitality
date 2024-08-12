using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

public class ConfigDbContext : DbContext
{
    private readonly string _connectionString;
    public ConfigDbContext(IConfiguration configuration)
    {
        var connectionString = configuration["Modules:Users:Database"];

        _connectionString =
            @"Server=RONMURP4\SqlExpress;Database=Msh1;User Id=ron;Password=Sooty@1234;TrustServerCertificate=true;";
    }
    public DbSet<Config> Configs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
}