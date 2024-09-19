using System.Text;
using System.Xml;
using System.Xml.Linq;

public static class XmlExtensionMethods
{
	/// <summary>
	/// Change the format of XML text to indented or none
	/// </summary>
	/// <param name="sbInput"></param>
	/// <param name="format"></param>
	/// <returns></returns>
	public static StringBuilder FormatXml(this StringBuilder sbInput, Formatting format)
	{
		var sbOutput = new StringBuilder();

		var xdoc = XDocument.Parse(sbInput.ToString());

		using (var m = new MemoryStream())
		{
			using (var w = new XmlTextWriter(m, new UTF8Encoding(false)))
			{
				w.Formatting = format;

				xdoc.WriteTo(w);
				w.Flush();

				m.Position = 0;
				using (var sr = new StreamReader(m))
				{
					sbOutput.Append(sr.ReadToEnd());

					return sbOutput;
				}
			}
		}
	}

	/// <summary>
	/// Change the format of XML text to indented or none
	/// </summary>
	/// <param name="sInput"></param>
	/// <param name="format"></param>
	/// <returns></returns>
	public static StringBuilder FormatXml(this string sInput, Formatting format) => FormatXml(new StringBuilder(sInput), format);
}