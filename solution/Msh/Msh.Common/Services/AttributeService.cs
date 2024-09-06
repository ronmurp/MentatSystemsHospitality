using System.Reflection;

namespace Msh.Common.Services;

/// <summary>
/// Get attributes from PropertyInfo
/// </summary>
public class AttributeService
{
	public string GetDescriptionAttribute(PropertyInfo prop)
	{
		var attributes = prop.GetCustomAttributes(true);
		var attr = GetAttribute(attributes, "DescriptionAttribute");
		if (attr != null)
		{
			var y = (System.ComponentModel.DescriptionAttribute)attr;
			return y.Description;
		}
		return string.Empty;
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
		var attributes = prop.GetCustomAttributes(true);
		var attr = GetAttribute(attributes, "InfoAttribute");
		if (attr != null)
		{
			var y = (Msh.Common.Attributes.InfoAttribute)attr;
			return y.Info;

		}
		return string.Empty;
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
}