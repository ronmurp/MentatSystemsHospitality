using Microsoft.EntityFrameworkCore;
using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

/// <summary>
/// This context is only for configuration data
/// </summary>
public class ConfigDbContext : DbContext
{
 
    private readonly string? _connectionString;

    public ConfigDbContext(DbContextOptions<ConfigDbContext> options)
        : base(options)
    {
        
    }

    /// <summary>
    /// Used for testing
    /// </summary>
    /// <param name="connectionString"></param>
    public ConfigDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Default OnConfiguring unless connection string is passed during testing
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            base.OnConfiguring(optionsBuilder);
            return;
        }
        
        optionsBuilder.UseSqlServer(_connectionString);

    }
    

    /// <summary>
    /// Common configs location, where Content is json for specific config classes
    /// Published version
    /// </summary>
    public DbSet<Config> Configs { get; set; }

    /// <summary>
    /// Common configs location, where Content is json for specific config classes
    /// Published version. This is the data used by customer facing apps
    /// </summary>
    public DbSet<ConfigPub> ConfigsPub { get; set; }

    /// <summary>
    /// Common configs location, where Content is json for specific config classes
    /// Archive version. This is where archive and other useful copies are held
    /// </summary>
    public DbSet<ConfigPub> ConfigsArchive { get; set; }
}