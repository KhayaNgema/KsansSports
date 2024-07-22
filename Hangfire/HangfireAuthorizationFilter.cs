using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}
