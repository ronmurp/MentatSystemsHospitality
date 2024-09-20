using System.ComponentModel.DataAnnotations;

namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Defines logging to XML
/// </summary>
public class LogXmlConfig
{
	[Display(Name="File Path")]
	public string? Path { get; set; } // Path to SOAP log folder ...


	[Display(Name="Relative Path")]
	public bool RelativePath { get; set; } // True: Relative to App_Data. False: Absolute path 


	public List<LogXmlConfigItem> Items { get; set; } = [];

	[Display(Name= "Enable Soap Trace")]
	public bool EnableSoapTrace { get; set; }


	[Display(Name = "Redact")]
	public bool Redact { get; set; } = true;

	[Display(Name = "Log Destination")]
	public LogDestination LogDestination { get; set; }
}

public enum LogDestination
{
	Database,
	File
}