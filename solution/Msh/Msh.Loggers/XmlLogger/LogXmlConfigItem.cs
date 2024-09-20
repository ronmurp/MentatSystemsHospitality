﻿namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Defines a particular log item, it's specific filename (formatted), and whether enabled or not
/// </summary>
public class LogXmlConfigItem
{
	public string Key { get; set; }
	public bool Enabled { get; set; }
	public string Filename { get; set; }
	public string MessageName { get; set; } // In order to find it for SoapTrace
	public bool FullTrace { get; set; }
}