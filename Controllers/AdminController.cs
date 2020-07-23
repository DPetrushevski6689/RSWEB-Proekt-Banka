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
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        private readonly BankaContext _context;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> hash, IPasswordValidator<AppUser> pv, IUserValidator<AppUser> uv, BankaContext ctx)
        {
            userManager = usrMgr;
            passwordHasher = hash;
            passwordValidator = pv;
            userValidator = uv;
            _context = ctx;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            IQueryable<AppUser> users = userManager.Users.OrderBy(u => u.Email);
            return View(users);
        }

        public IActionResult VrabotenProfile(int Id)
        {
            AppUser user = userManager.Users.FirstOrDefault(u => u.VrabotenId == Id);
            Employee vraboten = _context.Employee.Where(s => s.Id == Id).FirstOrDefault();

            if(vraboten!=null)
            {
                ViewData["FullName"] = vraboten.FullName;
                ViewData["VrabotenId"] = vraboten.Id;
            }
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VrabotenProfile(int Id, string email, string password)
        {
            AppUser user = userManager.Users.FirstOrDefault(u => u.VrabotenId == Id);
            if(user!=null)
            {
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                user.Email = email;
                user.UserName = email;

                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }

                validUser = await userValidator.ValidateAsync(userManager, user);
                if(!validUser.Succeeded)
                {
                    Errors(validUser);
                }

                if(!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }

                if(validUser!=null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(VrabotenProfile), new { Id });
                    else
                        Errors(result);
                }
            }
            else
            {
                AppUser newuser = new AppUser();
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                newuser.Email = email;
                newuser.UserName = email;
                newuser.VrabotenId = Id;
                newuser.Role = "Vraboten";

                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError("", "Email cannot be empty");

                validUser = await userValidator.ValidateAsync(userManager, newuser);
                if (!validUser.Succeeded)
                    Errors(validUser);

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                    if (validPass.Succeeded)
                        newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if(validUser != null && validUser.Succeeded && validPass !=null && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.CreateAsync(newuser, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newuser, "Vraboten");
                        return RedirectToAction(nameof(VrabotenProfile), new { Id });
                    }
                    else
                        Errors(result);
                }
                user = newuser;
            }
            Employee vraboten = _context.Employee.Where(s => s.Id == Id).FirstOrDefault();
            if(vraboten!=null)
            {
                ViewData["FullName"] = vraboten.FullName;
                ViewData["VrabotenId"] = vraboten.Id;
            }
            return View(user);
        }


        public IActionResult KorisnikProfile(int Id)
        {
            AppUser user = userManager.Users.FirstOrDefault(u => u.KorisnikId == Id);
            Korisnik korisnik = _context.Korisnik.Where(s => s.Id == Id).FirstOrDefault();

            if (korisnik != null)
            {
                ViewData["FullName"] = korisnik.FullName;
                ViewData["KorisnikId"] = korisnik.Id;
            }
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return View(null);
            }
        }


        /****************************************************************************************************/

        [HttpPost]
        public async Task<IActionResult> KorisnikProfile(int Id, string email, string password)
        {
            AppUser user = userManager.Users.FirstOrDefault(u => u.KorisnikId == Id);
            if (user != null)
            {
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                user.Email = email;
                user.UserName = email;

                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }

                validUser = await userValidator.ValidateAsync(userManager, user);
                if (!validUser.Succeeded)
                {
                    Errors(validUser);
                }

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    else
                        Errors(validPass);
                }

                if (validUser != null && validUser.Succeeded && (string.IsNullOrEmpty(password) || validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(KorisnikProfile), new { Id });
                    else
                        Errors(result);
                }
            }
            else
            {
                AppUser newuser = new AppUser();
                IdentityResult validUser = null;
                IdentityResult validPass = null;

                newuser.Email = email;
                newuser.UserName = email;
                newuser.KorisnikId = Id;
                newuser.Role = "Korisnik";

                if (string.IsNullOrEmpty(email))
                    ModelState.AddModelError("", "Email cannot be empty");

                validUser = await userValidator.ValidateAsync(userManager, newuser);
                if (!validUser.Succeeded)
                    Errors(validUser);

                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, newuser, password);
                    if (validPass.Succeeded)
                        newuser.PasswordHash = passwordHasher.HashPassword(newuser, password);
                    else
                        Errors(validPass);
                }
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (validUser != null && validUser.Succeeded && validPass != null && validPass.Succeeded)
                {
                    IdentityResult result = await userManager.CreateAsync(newuser, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newuser, "Korisnik");
                        return RedirectToAction(nameof(KorisnikProfile), new { Id });
                    }
                    else
                        Errors(result);
                }
                user = newuser;
            }
            Korisnik korisnik = _context.Korisnik.Where(s => s.Id == Id).FirstOrDefault();
            if (korisnik != null)
            {
                ViewData["FullName"] = korisnik.FullName;
                ViewData["KorisnikId"] = korisnik.Id;
            }
            return View(user);
        }








        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}