using Msh.Common.Models.Configuration;

namespace Msh.Common.Services;

/// <summary>
/// A place to record various states that other records can be recorded in.
/// </summary>
public interface IConfigStateRepo
{
	Task<List<ConfigState>> GetConfigState();

	Task SaveConfigState(List<ConfigState> list);
}