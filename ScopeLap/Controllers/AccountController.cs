using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models;
using ScopeLap.Tools;

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


    }
}
