using System;
using Microsoft.AspNet.Razor.Runtime.TagHelpers;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.TagHelpers;

namespace RepoManager
{
    [HtmlElementName("li")]
    public class NavBarLinkTagHelper : TagHelper
    {
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string ActiveClassAtttributeName = "active-class";

        [Activate]
        protected internal ViewContext ViewContext { get; set; }

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(ActiveClassAtttributeName)]
        public string ActiveClass { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(ActiveClass))
            {
                var descriptor = ViewContext.ActionDescriptor as ControllerActionDescriptor;
                if (descriptor == null)
                {
                    return;
                }

                if (descriptor.Name == Action && descriptor.ControllerName == Controller)
                {
                    var builder = new TagBuilder(output.TagName);
                    builder.Attributes.Add("class", ActiveClass);
                    output.MergeAttributes(builder);
                }
            }
            base.Process(context, output);
        }
    }
}