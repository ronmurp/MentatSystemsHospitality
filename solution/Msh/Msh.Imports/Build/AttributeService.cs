using System.Reflection;
using System.Text.RegularExpressions;

namespace Msh.Imports.Build;

/// <summary>
/// Get attributes from PropertyInfo
/// </summary>
public class AttributeService
{
	public string GetDescriptionAttribute(PropertyInfo prop)
	{
		var description = string.Empty;

		var attributes = prop.GetCustomAttributes(true);
		var attr = GetAttribute(attributes, "DescriptionAttribute");
		if (attr != null)
		{
			var y = (System.ComponentModel.DescriptionAttribute)attr;
			if(!string.IsNullOrEmpty(y.Description))
				return y.Description;
		}
		attr = GetAttribute(attributes, "DisplayAttribute");
		if (attr != null)
		{
			var y = (System.ComponentModel.DataAnnotations.DisplayAttribute)attr;
			if (!string.IsNullOrEmpty(y.Name))
				return y.Name;
		}

		description = InsertSpace(prop.Name);

		return description;
	}

	public string GetCategoryAttribute(PropertyInfo prop)
	{
		var attributes = prop.GetCustomAttributes(true);
		var attr = GetAttribute(attributes, "CategoryAttribute");
		if (attr != null)
		{
			var y = (System.ComponentModel.CategoryAttribute)attr;
			return y.Category;
		}
		return string.Empty;
	}

	public string GetInfoAttribute(PropertyInfo prop)
	{
		var info = string.Empty;
		var attributes = prop.GetCustomAttributes(true);
		var attr = GetAttribute(attributes, "InfoAttribute");
		if (attr != null)
		{
			var y = (Msh.Common.Attributes.InfoAttribute)attr;
			info = y.Info;

		}

		if (string.IsNullOrEmpty(info))
		{
			info = InsertHyphen(prop.Name);
		}
		return info;
	}

	public string GetCssClassAttribute(PropertyInfo prop)
	{
		var attributes = prop.GetCustomAttributes(true);
		var attr = GetAttribute(attributes, "CssClassAttribute");
		if (attr != null)
		{
			var y = (Msh.Common.Attributes.CssClassAttribute)attr;
			return y.CssClass;

		}
		return string.Empty;
	}

	private object? GetAttribute(object[] att, string name)
	{
		if (att.Length > 0)
		{
			var s = att.FirstOrDefault(a => a.GetType().Name == name);
			if (s != null)
			{
				return s;
			}
		}
		return null;
	}

	private string InsertHyphen(string input)
	{
		// Regex pattern: Look for transitions from a lowercase letter followed by an uppercase letter
		return Regex.Replace(input, "(?<!^)([A-Z])", "-$1").ToLower();
	}
	private string InsertSpace(string input)
	{
		// Regex pattern: Look for transitions from a lowercase letter followed by an uppercase letter
		return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
	}
}