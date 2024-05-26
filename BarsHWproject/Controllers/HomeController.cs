using BarsHWproject.DI;
using BarsHWproject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BarsHWproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChekUser _chekUser;

        public HomeController(ILogger<HomeController> logger, IChekUser chekUser)
        {
            _logger = logger;
            _chekUser = chekUser;
        }
        [Authorize(Roles = "User")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            string? role = _chekUser.CheckUser(login, password);
            if (role == null)
            {
                _logger.LogInformation("user not found");
                return RedirectToAction("Error");
            }

            else
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, login), new Claim(ClaimsIdentity.DefaultRoleClaimType, role) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await Response.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                _logger.LogInformation("user is logged in system");
            }

            return RedirectToAction("index"); ;
        }

        [Authorize(Roles = "Admin")]
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
