using Msh.Common.Attributes;

namespace Msh.Common.Services;

public class PropertyValueService : AttributeService
{
	public List<PropertyValues> GetProperties(Type classType)
	{
		var list = new List<PropertyValues>();

		var properties = classType.GetProperties();
		foreach (var prop in properties)
		{
			var pv = new PropertyValues
			{
				Name = prop.Name,
				DataType = $"{prop.PropertyType}",
				Category = GetCategoryAttribute(prop)
			};


			switch (pv.DataType)
			{
				case "System.String" when pv.Category == "Html":
					break;

				case "System.String" when pv.Category == "TextArea":
					break;

				case "System.String":
					break;

				case "System.Decimal":
					break;

				case "System.Boolean":
					break;

				case "System.Int32":
					break;

				case "System.DateTime":
					break;

				default:
					break;
			}

			list.Add(pv);
		}

		return list;
	}
}