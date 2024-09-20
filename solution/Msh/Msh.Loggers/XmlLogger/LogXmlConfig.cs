namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Defines logging to XML
/// </summary>
public class LogXmlConfig
{
	public string Path { get; set; } // Path to SOAP log folder ...
	public bool RelativePath { get; set; } // True: Relative to App_Data. False: Absolute path 
	public List<LogXmlConfigItem> Files { get; set; }
	public bool EnableSoapTrace { get; set; }
	public bool Redact { get; set; } = true;
}