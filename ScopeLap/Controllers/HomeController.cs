using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models;
using ScopeLap.Tools;
using System.Diagnostics;
using System.Security.Claims;
using System;

namespace ScopeLap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScopeLapDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ScopeLapDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Identity.Name != null)
            { 
                return RedirectToAction("UserPage", "Account");
            }
 
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _dbContext.Accounts
                    .Where(x =>
                        (x.Username == model.UsernameOrEmail || x.Email == model.UsernameOrEmail)
                        && x.HashPass == PassString.ComputeSHA512(model.HashPass)).FirstOrDefaultAsync();
                if (user != null)
                {
                    //cookie
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim("Name", user.Firstname),
                            new Claim("Id", user.Id.ToString()),
                            new Claim(ClaimTypes.Role, "User")
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("UserPage", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }
            }
            return View();
        }
    


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
