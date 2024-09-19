using Msh.Imports.Utilities;
using System.Text;
using System.Text.RegularExpressions;

namespace Msh.Imports.Build;

public class EditPageBuildService : AttributeService
{
	private readonly string _templatesPath;
	public EditPageBuildService()
	{
		_templatesPath = Path.Combine(FileUtilities.GetProjectPath(), "Build", "CshtmlEditFields");
	}

	public StringBuilder BuildEditPage(Type classType, string publishName)
	{
		
		var editPageOutput = new StringBuilder();

		var editPageFilename = Path.Combine(_templatesPath, "EditPage.cshtml");

		var pageText = File.ReadAllText(editPageFilename);

		var sb = BuildFields(classType);

		pageText = pageText.Replace("{InputFields}", sb.outputFields.ToString());
		pageText = pageText.Replace("{InfoFields}", sb.outputInfo.ToString());
		pageText = pageText.Replace("{PublishName}", publishName);
		pageText = pageText.Replace("{Model}", classType.ToString());

		editPageOutput.AppendLine(pageText);

		return editPageOutput;

	}

	public (StringBuilder outputFields, StringBuilder outputInfo) BuildFields(Type classType)
	{
		

		var outputFields = new StringBuilder();
		var outputInfo = new StringBuilder();

		var properties = classType.GetProperties();

		

		foreach (var prop in properties)
		{
			var file = string.Empty;

			var category = GetCategoryAttribute(prop);

			switch ($"{prop.PropertyType}")
			{
				
				case "System.String" when category == "Html":
					file = "EditFieldTextArea.cshtml";
					break;

				case "System.DateOnly":
					file = "EditFieldDate.cshtml";
					break;

				case "System.String" when category == "TextArea":
					file = "EditFieldTextArea.cshtml";
					break;

				case "System.String":
					file = "EditFieldText.cshtml";
					break;

				case "System.Decimal":
					file = "EditFieldText.cshtml";
					break;

				case "System.Boolean":
					file = "EditFieldCheck.cshtml";
					break;

				case "System.Int32":
					file = "EditFieldInt.cshtml";
					break;

				default:
					if(prop.PropertyType.IsEnum)
					{
						file = "EditFieldEnum.cshtml";
					}
					break;
			}

			if (string.IsNullOrEmpty(file)) continue;

			var filenameFields = Path.Combine(_templatesPath, file);

			var textField = File.ReadAllText(filenameFields);

			textField = textField.Replace("{Property}", prop.Name);
			textField = textField.Replace("{PropertyDescription}", GetDescriptionAttribute(prop));
			textField = textField.Replace("{PropertyInfo}", GetInfoAttribute(prop));
			textField = textField.Replace("{CssClass}", GetCssClassAttribute(prop));
			textField = textField.Replace("{PropertyType}",$"{prop.PropertyType}");

			outputFields.AppendLine(textField);

			var filenameInfo = Path.Combine(_templatesPath, "EditInfo.cshtml");
			var textInfo = File.ReadAllText(filenameInfo);
			textInfo = textInfo.Replace("{InfoId}", GetInfoAttribute(prop));
			textInfo = textInfo.Replace("{PropertyDescription}", GetDescriptionAttribute(prop));

			outputInfo.AppendLine(textInfo);
		}

		return (outputFields, outputInfo);
	}

	
}