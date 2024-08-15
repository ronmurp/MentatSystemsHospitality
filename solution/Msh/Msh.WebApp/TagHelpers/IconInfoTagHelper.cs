using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Msh.WebApp.TagHelpers;

[HtmlTargetElement("info-icon")]
public class IconInfoTagHelper(IHtmlGenerator generator) : AnchorTagHelper(generator)
{
	[HtmlAttributeName("name")]
	public string IconInfoName { get; set; } = string.Empty;

	public override void Process(TagHelperContext context, TagHelperOutput output)
	{
		output.TagName = "a";
		output.TagMode = TagMode.StartTagAndEndTag;
		output.Attributes.Add("href", $"javascript:mshMethods.showInfo('{IconInfoName}')");
		output.Content.SetHtmlContent("<i class=\"fa fa-info-circle\"></i>");
		
		base.Process(context, output);
	}
}