using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Data;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided generic routes
    /// </summary>
    public partial class GenericUrlRouteProvider : BaseRouteProvider, IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var pattern = GetLanguageRoutePattern();

            //default routes
            //these routes are not generic, they are just default to map requests that don't match other patterns, 
            //but we define them here since this route provider is with the lowest priority, to allow to add additional routes before them
            if (!string.IsNullOrEmpty(pattern))
            {
                endpointRouteBuilder.MapControllerRoute(name: "DefaultWithLanguageCode",
                    pattern: $"{pattern}/{{controller=Home}}/{{action=Index}}/{{id?}}");
            }
            endpointRouteBuilder.MapDynamicControllerRoute<SlugRouteTransformer>("{**SeName}");

            endpointRouteBuilder.MapControllerRoute(name: "Default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            //generic routes
            var genericPattern = $"{pattern}/{{SeName}}";

           //generic URLs
            endpointRouteBuilder.MapControllerRoute(
                name: "GenericUrl",
                pattern: "{GenericSeName}",
                new { controller = "Common", action = "GenericUrl" });

            //define this routes to use in UI views (in case if you want to customize some of them later)
            endpointRouteBuilder.MapControllerRoute("ProductWithPath", "{Path}/" + pattern, 
                new { controller = "Product", action = "ProductDetails" });

            endpointRouteBuilder.MapControllerRoute("CategoryWithPath", "{Path}/" + pattern, 
                new { controller = "Catalog", action = "Category" });

            endpointRouteBuilder.MapControllerRoute("BlogPostWithPath", "{Path}/" + pattern, 
                new { controller = "Blog", action = "BlogPost" });

            endpointRouteBuilder.MapControllerRoute("TopicWithPath", "{Path}/" + pattern, 
                new { controller = "Topic", action = "TopicDetails" });
                
            endpointRouteBuilder.MapControllerRoute(name: "GenericUrl",
                pattern: "{genericSeName}",
                defaults: new { controller = "Common", action = "GenericUrl" });

            endpointRouteBuilder.MapControllerRoute(name: "GenericUrlWithParameter",
                pattern: "{genericSeName}/{genericParameter}",
                defaults: new { controller = "Common", action = "GenericUrl" });

            endpointRouteBuilder.MapControllerRoute(name: "Product",
                pattern: genericPattern,
                defaults: new { controller = "Product", action = "ProductDetails" });

            endpointRouteBuilder.MapControllerRoute(name: "Category",
                pattern: genericPattern,
                defaults: new { controller = "Catalog", action = "Category" });

            endpointRouteBuilder.MapControllerRoute(name: "Manufacturer",
                pattern: genericPattern,
                defaults: new { controller = "Catalog", action = "Manufacturer" });

            endpointRouteBuilder.MapControllerRoute(name: "Vendor",
                pattern: genericPattern,
                defaults: new { controller = "Catalog", action = "Vendor" });

            endpointRouteBuilder.MapControllerRoute(name: "NewsItem",
                pattern: genericPattern,
                defaults: new { controller = "News", action = "NewsItem" });

            endpointRouteBuilder.MapControllerRoute(name: "BlogPost",
                pattern: genericPattern,
                defaults: new { controller = "Blog", action = "BlogPost" });

            endpointRouteBuilder.MapControllerRoute(name: "Topic",
                pattern: genericPattern,
                defaults: new { controller = "Topic", action = "TopicDetails" });

            endpointRouteBuilder.MapControllerRoute(name: "ProductsByTag",
                pattern: genericPattern,
                defaults: new { controller = "Catalog", action = "ProductsByTag" });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        /// <remarks>
        /// it should be the last route. we do not set it to -int.MaxValue so it could be overridden (if required)
        /// </remarks>
        public int Priority => -1000000;

        #endregion
    }
}