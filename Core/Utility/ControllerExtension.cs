
using Microsoft.AspNetCore.Mvc;

public static class ControllerBaseExtension
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