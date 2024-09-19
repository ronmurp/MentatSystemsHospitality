using Msh.Common.Models;

namespace Msh.Common.Services;

public interface ISwitchListLoader
{
	List<OwsSwitch> SwitchList { get; }
	List<OwsSwitch> Load(string filename);
	void Save(string filename, List<OwsSwitch> list);
}