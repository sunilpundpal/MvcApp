using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcAppJwtAuth.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MvcAppJwtAuth.Core.Constants;
using MvcAppJwtAuth.Services;

namespace MvcAppJwtAuth.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public HomeController(
        IUserService userService,
        ITokenService tokenService,
        ILogger<HomeController> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    // [Authorize(Roles = UserRolesConstant.Admin)]
    public IActionResult Privacy()
    {
        ViewData["JWToken"] = HttpContext.Session.GetString("JWToken");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // [Authorize(Roles = UserRolesConstant.Admin)]
    [Authorize]
    public IActionResult Secured()
    {
        ViewData["JWToken"] = HttpContext.Session.GetString("JWToken");
        return View();
    }

    [HttpGet("Login")]
    public IActionResult Login(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost("Login")]
    public IActionResult LoginAsync(string username, string password, string? returnUrl)
    {
        var validUser = _userService.Authenticate(username, password);

        if (validUser != null)
        {
            var generatedToken = _tokenService.BuildToken(validUser);

            if (generatedToken != null)
            {
                HttpContext.Session.SetString("JWToken", generatedToken);
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return View("Index");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
    }

    // [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return View();
    }

    [HttpGet("AccessDenied")]
    public ActionResult AccessDenied()
    {
        ViewData[ClaimTypes.Name] = HttpContext.User.Identity?.Name;
        return View();
    }

    [HttpGet]
    [Authorize]
    public IActionResult Dashboard()
    {
        var userName = HttpContext.UserName();
        ViewData["UserName"] = HttpContext.User.Identity?.Name;
        return View();
    }

}
