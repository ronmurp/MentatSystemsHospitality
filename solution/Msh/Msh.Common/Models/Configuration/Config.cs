using System.ComponentModel.DataAnnotations;

namespace Msh.Common.Models.Configuration;

/// <summary>
/// General configuration saved in the database, where Content is json representing a class
/// </summary>
public class Config
{
    /// <summary>
    /// A name given to the config record - typically the data type or class name.
    /// </summary>
    [Key]
    public string ConfigType { get; set; } = string.Empty;

    /// <summary>
    /// json that deserializes to an object
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Admin notes
    /// </summary>
    public string? Notes { get; set; }
}