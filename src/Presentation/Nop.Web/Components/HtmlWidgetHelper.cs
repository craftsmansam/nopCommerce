using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Components
{
    public static class HtmlWidgetHelper
    {
        public static async Task<string> ParseWidgetAndComponentsFromTextAsync(this IViewComponentHelper component, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var groupName = "widgetName";
            var sb = await ParseIndividualComponentAsync(text, $"<p>##(?<{groupName}>[a-zA-Z ]+)##</p>", groupName, 
                                                    async widgetZone => await component.InvokeAsync("Widget", new {widgetZone}));
            
            text = sb.ToString();
            groupName = "componentName";
            sb = await ParseIndividualComponentAsync(text, $"<p>@@(?<{groupName}>[a-zA-Z_ ]+)@@</p>", groupName,
                async componentName =>
                {
                    // e.g. CapacitiesChart_Angle
                    if (componentName.Contains("_"))
                    {
                        var parameters = componentName.Split('_');
                        return await component.InvokeAsync(parameters[0], parameters[1]);
                    }

                    return await component.InvokeAsync(componentName, null);
                });

            return sb.ToString();
        }

        private static async Task<StringBuilder> ParseIndividualComponentAsync(string text, string regexPattern, string regexGroupName, Func<string, Task<IHtmlContent>> invokeComponentFunc)
        {
            var sb = new StringBuilder();
            var regex = new Regex(regexPattern);
            var match = regex.Match(text);
            while (match.Success)
            {
                sb.Append(text.Substring(0, match.Index));

                var widgetZone = match.Groups[regexGroupName].Value;

                var htmlContent = await invokeComponentFunc(widgetZone);
                await using var writer = new StringWriter();
                htmlContent.WriteTo(writer, HtmlEncoder.Default);
                var asString = writer.ToString();
                
                sb.Append(asString);
                
                text = text.Substring(match.Index + match.Length);

                match = regex.Match(text);
            }

            sb.Append(text);
            return sb;
        }
    }
}
