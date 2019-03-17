using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibOnline.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using LibOnline.Models.Account;

namespace LibOnline.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;
        public AccountController(ApplicationContext context)
        {
            _context = context;
        }//AccountController

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.Login == model.Login);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { Login = model.Login, Email = model.Email, Password = model.ConvertPasswosdToMD5(model.Password) };
                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Reader");
                    if (userRole != null)
                    {
                        user.Role = userRole;
                        user.IsActive = true;
                    }//if
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                //ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }//if

            ViewBag.result = "error";
            return View(model);
        }
        [HttpGet]
        #endregion

        #region Recovery
        public IActionResult Recovery()
        {
            return View();
        }//Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recovery(RecoveryModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync(u =>
                                                 u.Email == model.Email &&
                                                 u.IsActive == true);
                if (user != null)
                {
                    user.Password = model.ConvertPasswosdToMD5(model.Password);

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }//if
              
                //ModelState.AddModelError("", "Некорректные E-mail и(или) пароль");
            }//if

            ViewBag.result = "error";
            return View(model);
        }//Login
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }//Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                            .Include(u => u.Role)
                            .FirstOrDefaultAsync(u => 
                                                 u.Login == model.Login && 
                                                 u.Password == model.ConvertPasswosdToMD5(model.Password) &&
                                                 u.IsActive == true);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }//if
            
                //ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }//if


            ViewBag.result = "error";
            return View(model);
        }//Login
        #endregion

        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }// LogOut
        #endregion 

        #region Аутентификация в Claim
        // Аутентификация в Claim
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                //new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }//Authenticate
        #endregion

    }// AccountController
}// namsespace