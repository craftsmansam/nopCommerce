using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Nop.Web.Infrastructure;

public class CraftsmanUrlHelper : UrlHelperBase
{
    private readonly LinkGenerator _linkGenerator;

    public CraftsmanUrlHelper(
        ActionContext actionContext,
        LinkGenerator linkGenerator)
        : base(actionContext)
    {
        ArgumentNullException.ThrowIfNull(linkGenerator);

        _linkGenerator = linkGenerator;
    }

    /// <inheritdoc />
    public override string? Action(UrlActionContext urlActionContext)
    {
        ArgumentNullException.ThrowIfNull(urlActionContext);

        var values = GetValuesDictionary(urlActionContext.Values);

        if (urlActionContext.Action == null)
        {
            if (!values.ContainsKey("action") &&
                AmbientValues.TryGetValue("action", out var action))
            {
                values["action"] = action;
            }
        }
        else
        {
            values["action"] = urlActionContext.Action;
        }

        if (urlActionContext.Controller == null)
        {
            if (!values.ContainsKey("controller") &&
                AmbientValues.TryGetValue("controller", out var controller))
            {
                values["controller"] = controller;
            }
        }
        else
        {
            values["controller"] = urlActionContext.Controller;
        }

        var path = _linkGenerator.GetPathByRouteValues(
            ActionContext.HttpContext,
            routeName: null,
            values,
            fragment: urlActionContext.Fragment == null
                ? FragmentString.Empty
                : new FragmentString("#" + urlActionContext.Fragment));
        return GenerateUrl(urlActionContext.Protocol, urlActionContext.Host, path);
    }

    /// <inheritdoc />
    public override string? RouteUrl(UrlRouteContext routeContext)
    {
        ArgumentNullException.ThrowIfNull(routeContext);

        var path = _linkGenerator.GetPathByRouteValues(
            ActionContext.HttpContext,
            routeContext.RouteName,
            routeContext.Values,
            fragment: routeContext.Fragment == null
                ? FragmentString.Empty
                : new FragmentString("#" + routeContext.Fragment));
        var generateUrl = GenerateUrl(routeContext.Protocol, routeContext.Host, path);
        return generateUrl?.Replace("%2F", "/");
    }
}

public class CraftsmanUrlHelperFactory : IUrlHelperFactory
{
    public IUrlHelper GetUrlHelper(ActionContext context)
    {
        var httpContext = context.HttpContext;

        if (httpContext == null)
        {
            throw new ArgumentException($"Property {nameof(ActionContext.HttpContext)} cannot be null!");
        }

        if (httpContext.Items == null)
        {
            throw new ArgumentException($"Property {nameof(HttpContext.Items)} cannot be null!");
        }

        ArgumentNullException.ThrowIfNull(context);

        // Perf: Create only one UrlHelper per context
        if (httpContext.Items.TryGetValue(typeof(IUrlHelper), out var value) && value is IUrlHelper urlHelper)
        {
            return urlHelper;
        }

        var endpoint = httpContext.GetEndpoint();
        if (endpoint != null)
        {
            var services = httpContext.RequestServices;
            var linkGenerator = services.GetRequiredService<LinkGenerator>();

            urlHelper = new CraftsmanUrlHelper(
                context,
                linkGenerator);
        }
        else
        {
            urlHelper = new UrlHelper(context);
        }

        httpContext.Items[typeof(IUrlHelper)] = urlHelper;

        return urlHelper;
    }
}