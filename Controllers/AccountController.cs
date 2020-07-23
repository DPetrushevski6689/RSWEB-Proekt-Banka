using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banka.Data;
using Banka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banka.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        private readonly BankaContext _context;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr,IPasswordHasher<AppUser> ph,IPasswordValidator<AppUser> pv,
            IUserValidator<AppUser> uv, BankaContext ctx)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            passwordHasher = ph;
            passwordValidator = pv;
            userValidator = uv;
            _context = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await userManager.FindByEmailAsync(login.Email);
                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        if((await userManager.IsInRoleAsync(appUser, "Administrator")))
                        {
                            return RedirectToAction("Index", "Korisnici", null);
                            //return Redirect(login.ReturnUrl ?? "/");
                        }
                        if ((await userManager.IsInRoleAsync(appUser, "Korisnik")))
                        {
                            return RedirectToAction("Index", "Korisnik", new { id = appUser.KorisnikId});
                        }
                        if ((await userManager.IsInRoleAsync(appUser, "Vraboten")))
                        {
                            return RedirectToAction("Index", "Vraboten", new { id = appUser.VrabotenId });
                        }
                    }
                        
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}