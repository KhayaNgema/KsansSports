using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System.Composition;
using System.Security.Claims;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
 
        var httpContext = context.GetHttpContext();

        if (!httpContext.User.Identity.IsAuthenticated)
        {
            return false;
        }

        if (httpContext.User.IsInRole("Sport Administrator"))
        {
            return true;
        }

        return false;
    }
}
