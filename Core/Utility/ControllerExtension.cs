
using Microsoft.AspNetCore.Mvc;

public static class HttpContextExtensions
{
    public static string UserName(this HttpContext httpContext)
    {
        if (httpContext != null)
        {
            return httpContext.User.Identity?.Name;
        }
        else
            return string.Empty;
    }
}