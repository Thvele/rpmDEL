using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rpmDEL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace rpmDEL.Controllers
{
    public class AccController : Controller
    {
        private ApplicationContext db;
        private IWebHostEnvironment _app;

        public AccController(ApplicationContext context, IWebHostEnvironment app)
        {
            db = context;
            _app = app;
        }

        public IActionResult Index()
        {
            return RedirectToAction("SignIn", "signin");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterModel model, IFormFile file)
        {
            if(ModelState.IsValid)
            {
                user user = await db._user.FirstOrDefaultAsync(u => u._login == model._login);
                if (user == null)
                {

                    FileModel newFile = null;

                    if (file != null)
                    {
                        string path = "/files/" + file.FileName;
                        using (var fileStream = new FileStream(_app.WebRootPath + path, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        newFile = new FileModel
                        {
                            _name = file.FileName,
                            _path = path
                        };
                        db.files.Add(newFile);
                    }

                    user = new user { _login = model._login, _pass = model._pass, _EMail = model._EMail, _Name = model._Name, _Fam = model._Fam, _Surname = model._Surname, _regday = DateTime.Now };
                    role userRole = await db.roles.FirstOrDefaultAsync(r => r.Name == "user");

                    if (newFile != null)
                        user._imgpath = newFile._path;
                    else
                        user._imgpath = "/images/Icon.jpg";

                    if (userRole != null)
                        user.Role = userRole;

                    db._user.Add(user);
                    await db.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("signin", "acc");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Не удалось подтвердить пароль!");
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                user user = await db._user.Include(u => u.Role).FirstOrDefaultAsync(u => u._login == model._login && u._pass == model._pass);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("profile", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn", "Acc");
        }

        private async Task Authenticate(user user)
        {

            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user._login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim(ClaimTypes.SerialNumber, user._ID.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
        }
    }
}
