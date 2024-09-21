using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.OwsErrors;

namespace Msh.Opera.Ows.Cache;

public interface IOwsCacheService
{
	Task<OwsConfig> GetOwsConfig();

	void ReloadOwsConfig();

	Task<OwsErrorList> GetOwsErrorList();

	void ReloadOwsErrorList();
}


