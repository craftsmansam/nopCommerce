using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;

namespace Nop.Services.Craftsman;

/// <summary>
/// Represents middleware that checks whether request is for keep alive
/// </summary>
public partial class UrlModifierMiddleware
{
    #region Fields

    protected readonly RequestDelegate _next;

    #endregion

    #region Ctor

    public UrlModifierMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Invoke middleware actions
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <param name="webHelper">Web helper</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context, IWebHelper webHelper)
    {

        if (webHelper.GetThisPageUrl(false).Contains("%2F", StringComparison.InvariantCultureIgnoreCase))
        {
            context.Response.Redirect(webHelper.GetThisPageUrl(false).Replace("%2F", "/"));
        }
        

        //or call the next middleware in the request pipeline
        await _next(context);
    }

    #endregion
}