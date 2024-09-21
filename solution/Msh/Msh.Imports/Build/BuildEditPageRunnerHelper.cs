namespace Msh.Imports.Build;

public class BuildEditPageRunnerHelper
{
	public void BuildPage(Type classType, string controller, string action="{Action}")
	{
		var service = new EditPageBuildService();

		var output = service.BuildEditPage(classType, controller, action);

		Console.WriteLine(output.ToString());
	}
}