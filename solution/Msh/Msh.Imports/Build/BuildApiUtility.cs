using NUnit.Framework;

namespace Msh.Imports.Build;

public static class BuildApiUtility
{
	public static void BuildApiFile(ApiBuildModel apiBuildModel)
	{

		var content = File.ReadAllText(apiBuildModel.TemplateFile);

		var body = content.Replace("{{ModelName}}", apiBuildModel.ModelName);
		body = body.Replace("{{ModelNameSing}}", apiBuildModel.ModelNameSing);
		body = body.Replace("{{LocalRepo}}", apiBuildModel.LocalRepo);
		body = body.Replace("{{PrivateRepo}}", apiBuildModel.PrivateRepo);
		body = body.Replace("{{ApiRoot}}", apiBuildModel.ApiRoot);

		File.WriteAllText(apiBuildModel.OutputFile, body);

	}
}

public class ApiBuildModel
{

	public string ModelName { get; set; } = string.Empty;

	public string ModelNameSing { get; set; } = string.Empty;

	public string LocalRepo { get; set; } = string.Empty;
	public string PrivateRepo { get; set; } = string.Empty;

	public string ApiRoot { get; set; } = string.Empty; // api/discountsapi

	public string TemplateFile { get; set; } = string.Empty;

	public string OutputFile { get; set; } = string.Empty;


}

[TestFixture]
public class BuildApiRunner
{
	[Test]
	public void BuildDiscountsApi()
	{
		var projectPath = Utilities.FileUtilities.GetProjectPath();
		var sourceFile = Path.Combine(projectPath, "Build", "ApiPallFiles", "ApiFileTemplate.txt");
		var outputFile = @"C:\Temp\TempOutputCode.cs";

		BuildDiscountApi(sourceFile, outputFile);

	}

	[Test]
	public void BuildDiscountsApiPall()
	{
		var projectPath = Utilities.FileUtilities.GetProjectPath();
		var sourceFile = Path.Combine(projectPath, "Build", "ApiPallFiles", "ApiFileTemplatePall.txt");
		var outputFile = @"C:\Temp\TempOutputCodePall.cs";

		BuildDiscountApi(sourceFile, outputFile);
	}

	private void BuildDiscountApi(string sourceFile, string outputFile)
	{
		BuildApiUtility.BuildApiFile(new ApiBuildModel
		{

			TemplateFile = sourceFile,
			ModelName = "Discounts",
			ModelNameSing = "Discount",
			LocalRepo = "_discountsRepository",
			PrivateRepo = "discountsRepository",
			ApiRoot = "api/discountsapi",
			OutputFile = outputFile
		});
	}

}