using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models;
using ScopeLap.Tools;
using System.Security.Claims;

namespace ScopeLap.Controllers
{
    public class AccountController : Controller
    {
        private readonly ScopeLapDbContext _context;

        public AccountController(ScopeLapDbContext context) { 
            this._context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registration() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = new Account();
                account.Email = model.Email;
                account.Firstname = model.Firstname;
                account.Lastname = model.Lastname;
                account.Username = model.Username;
                account.HashPass = PassString.ComputeSHA512(model.HashPass);

                try
                {
                    _context.Accounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"Учетная запись для {account.Firstname} {account.Lastname} успешно создана. Можете войти в учетную запись.";

                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("","Данный e-mail уже зарегистрирован.");
                    return View(model);
                }
                
                return View();

            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Accounts
                    .Where(x => 
                        (x.Username == model.UsernameOrEmail || x.Email == model.UsernameOrEmail) 
                        && x.HashPass == PassString.ComputeSHA512(model.HashPass)).FirstOrDefaultAsync();
                if (user != null)
                {
                    //cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.Firstname),
                        new Claim(ClaimTypes.Role, "User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("SecurePage");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }

            }
            return View();
        }

        [Authorize]
        public IActionResult SecurePage() 
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View(); 
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }


    }
}
