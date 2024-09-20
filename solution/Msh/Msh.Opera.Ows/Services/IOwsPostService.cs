using System.Text;
using System.Xml.Linq;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services;

public interface IOwsPostService
{
	Task<(XDocument xdoc, string contents, OwsResult owsResult)> PostAsync(StringBuilder sb, string url, string sessionId = "");
}