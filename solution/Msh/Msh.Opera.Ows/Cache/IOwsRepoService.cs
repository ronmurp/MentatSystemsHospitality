using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.OwsErrors;

namespace Msh.Opera.Ows.Cache;

public interface IOwsRepoService
{
	Task<OwsConfig> GetOwsConfigAsync();
	Task SaveOwsConfigAsync(OwsConfig owsConfig);

	Task<OwsErrorList> GetOwsErrorListAsync();

	Task SaveOwsErrorListAsync(OwsErrorList owsErrorList);
}