using System.Text;
using System.Xml.Linq;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Mocks;

/// <summary>
/// A mock IOwsPostService that can be used to push dummy data back to OWS instead of making an OWS call
/// </summary>
public class OwsPostServiceMock : IOwsPostService
{
	private readonly string _appDataPath;

	public OwsPostServiceMock()
	{
            
	}

	public OwsPostServiceMock(string appDataPath)
	{
		_appDataPath = appDataPath;
	}
	public Task<(XDocument xdoc, string contents, OwsResult owsResult)> PostAsync(StringBuilder sb, string url, string sessionId = "")
	{
		throw new NotImplementedException();
	}

	public (XDocument xdoc, string contents, OwsResult owsResult) PostSync(StringBuilder sb, string url, string sessionId = "")
	{
		var filename = Path.Combine(_appDataPath, "TestData", "Test_OperaData", "FitAgent-Avail-Response-001.xml");

		var contents = File.ReadAllText(filename);

		return (null, contents, new OwsResult(true));
	}
        
}