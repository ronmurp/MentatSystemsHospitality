namespace Msh.Common.Models.OwsCommon;

public class InformationList(LovTypes lt) : List<InformationItem>
{
	public LovTypes LovType { get; set; } = lt;
}