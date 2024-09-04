using System.Reflection;
using System.Text;
using Msh.Imports.Utilities;
using NUnit.Framework;

namespace Msh.Imports.Build;

[TestFixture]
[Explicit]
public class FieldRunner
{
	[Test]
	public void Paths()
	{
		Console.WriteLine(FileUtilities.PathBaseDirectory);
		Console.WriteLine(FileUtilities.PathLocation());
		Console.WriteLine(FileUtilities.GetProjectPath());
	}

	[Test]
	[TestCase("")]
	public void BuildEditPage(string desc)
	{
		
	}

	
}