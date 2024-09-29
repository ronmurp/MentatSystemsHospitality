using System.Xml.Linq;
using Msh.Common.Models.OwsCommon;

/// <summary>
/// Extension methods use to support loading of XML values from XAttributes and XElements
/// </summary>
public static class LinqXmlExtensionMethods
{
	// Todo - This doesn't always work
	public static bool ElementExists(this XDocument xdoc, string name) => xdoc.Element(name) != null;

	public static string ValueA(this XAttribute attr)
	{
		return (attr == null) ? string.Empty : (attr.Value ?? string.Empty);
	}

	public static string ValueA(this XElement e, string defaultValue, string name)
	{
		var attr = e.Attribute(name);
		if (attr == null) return defaultValue;

		var output = e.Attribute(name).ValueA();

		return output ?? defaultValue;
	}

	public static string ValueA(this XElement e, string name)
	{
		var attr = e.Attribute(name);
		if (attr == null) return string.Empty;

		var output = e.Attribute(name).ValueA();

		return output ?? string.Empty;
	}

	public static bool ValueA(this XElement e, bool defaultValue, string name)
	{
		if (e?.Attribute(name) == null) return defaultValue;

		var temp = e.Attribute(name)?.Value ?? "false";

		return bool.TryParse(temp, out var output) ? output : defaultValue;
	}

	public static int ValueA(this XElement e, int defaultValue, string name)
	{
		if (e?.Attribute(name) == null) return defaultValue;

		var temp = e.Attribute(name)?.Value ?? "0";

		return int.TryParse(temp, out var output) ? output : defaultValue;
	}

	public static decimal ValueA(this XElement e, decimal defaultValue, string name)
	{
		if (e?.Attribute(name) == null) return defaultValue;

		var temp = e.Attribute(name)?.Value ?? "0";

		return decimal.TryParse(temp, out var output) ? output : defaultValue;
	}

	public static DateTime ValueA(this XElement e, DateTime defaultValue, string name)
	{
		if (e?.Attribute(name) == null) return defaultValue;

		var temp = e.Attribute(name)?.Value ?? "0001-01-01";

		return DateTime.TryParse(temp, out var output) ? output : defaultValue;
	}

	public static DateOnly ValueA(this XElement e, DateOnly defaultValue, string name)
	{
		if (e?.Attribute(name) == null) return defaultValue;

		var temp = e.Attribute(name)?.Value ?? "0001-01-01";

		return DateOnly.TryParse(temp, out var output) ? output : defaultValue;
	}


	public static string ValueE(this XElement e)
	{
		return e?.Value ?? string.Empty;
	}

	public static string ValueE(this XElement e, string name)
	{
		if (e?.Element(name) != null)
		{
			return e.Element(name)?.Value ?? string.Empty;
		}
		return string.Empty;
	}

	public static string ValueE(this XElement e, string defaultValue, string name)
	{
		if (e?.Element(name) != null)
		{
			return e.Element(name)?.Value ?? defaultValue;
		}
		return defaultValue;
	}

	public static bool ValueE(this XElement e, bool defaultValue, string name)
	{
		if (e?.Element(name) != null)
		{
			return bool.TryParse(e.Element(name).ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static int ValueE(this XElement e, int defaultValue, string name)
	{
		if (e?.Element(name) != null)
		{
			return int.TryParse(e.Element(name).ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static decimal ValueE(this XElement e, decimal defaultValue, string name)
	{
		if (e?.Element(name) != null)
		{
			return decimal.TryParse(e.Element(name).ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static decimal ValueE(this XElement e, decimal defaultValue)
	{
		if (e != null)
		{
			return decimal.TryParse(e.ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static DateTime ValueE(this XElement e, DateTime defaultValue, string name)
	{
		if (e?.Element(name) != null)
		{
			return DateTime.TryParse(e.Element(name).ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static DateTime ValueE(this XElement e, DateTime defaultValue)
	{
		if (e != null)
		{
			return DateTime.TryParse(e.ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static DateOnly ValueE(this XElement e, DateOnly defaultValue, string name)
	{
		if (e?.Element(name) != null)
		{
			return DateOnly.TryParse(e.Element(name).ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}

	public static DateOnly ValueE(this XElement e, DateOnly defaultValue)
	{
		if (e != null)
		{
			return DateOnly.TryParse(e.ValueE(), out var output) ? output : defaultValue;
		}
		return defaultValue;
	}


	/// <summary>
	/// Find an XElement in a set of nested elements.
	/// </summary>
	/// <param name="e">The element that contains the nested set</param>
	/// <param name="names">The set of nested element names, ending in a target element.</param>
	/// <returns>The target element, or a dummy target element.</returns>
	/// <remarks>
	/// The purpose is to return at least the target element or an empty dummy.
	/// This is because the other extensi methods above rely on an element being present.
	/// So, if either the target element is present, or even its names parents are missing,
	/// the other extension methods will still have an object to use, and will return their default values.
	/// This would typically be used when flatteing an xml structure to an object, where some nested xml is optional.
	/// </remarks>
	public static XElement Descendant(this XElement e, params string[] names)
	{
		for (var i = 0; i < names.Length; i++)
		{
			var name = names[i];
			if (e == null) continue;
			if (!e.Descendants(name).Any()) continue;
			e = e.Descendants(name).First();
			if (e != null && i == names.Length - 1)
			{
				return e;
			}
		}

		return new XElement(names.Last(), string.Empty);
	}

	public static string InnerXml(this XElement element)
	{
		var reader = element.CreateReader();
		reader.MoveToContent();
		return reader.ReadInnerXml();
	}

	/// <summary>
	/// Returns a SoapAmount structure common in OWS
	/// </summary>
	/// <param name="el"></param>
	/// <param name="elementName"></param>
	/// <returns></returns>
	public static SoapAmount GetSoapAmount(this XElement el, string elementName)
	{
		return new SoapAmount
		{
			Amount = el.Descendant(elementName).ValueE(0M),
			CurrencyCode = el.Descendant(elementName).ValueA("currencyCode"),
			DecimalPlaces = el.Descendant(elementName).ValueA(2, "decimals"),
		};
	}

	public static SoapAmount GetSoapAmount(this XElement el)
	{
		return new SoapAmount
		{
			Amount = el.ValueE(0M),
			CurrencyCode = el.ValueA("currencyCode"),
			DecimalPlaces = el.ValueA(2, "decimals"),
		};
	}

	/// <summary>
	/// The value amount is a named attribute rather than the element
	/// </summary>
	/// <param name="el"></param>
	/// <param name="amountAttribute"></param>
	/// <returns></returns>
	public static SoapAmount GetSoapAmountNamedAttribute(this XElement el, string amountAttribute) =>
		new SoapAmount
		{
			Amount = el.ValueA(0M, amountAttribute),
			CurrencyCode = el.ValueA("GBP", "currencyCode"),
			DecimalPlaces = el.ValueA(2, "decimals"),
		};
}