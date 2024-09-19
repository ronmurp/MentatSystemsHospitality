namespace Msh.Imports.Build;

public class BuildEditPageRunnerHelper
{
	public void BuildPage(Type classType, string publishName)
	{
		var service = new EditPageBuildService();

		var output = service.BuildEditPage(classType, publishName);

		Console.WriteLine(output.ToString());
	}
}