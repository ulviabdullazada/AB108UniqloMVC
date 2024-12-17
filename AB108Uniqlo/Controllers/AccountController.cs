using AB108Uniqlo.DataAccess;
using AB108Uniqlo.Extensions;
using AB108Uniqlo.Models;
using AB108Uniqlo.Services.Abstracts;
using AB108Uniqlo.ViewModels.Auths;
using AB108Uniqlo.Views.Account.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AB108Uniqlo.Controllers
{
    public class AccountController(UserManager<User> _userManager, SignInManager<User> _signInManager, RoleManager<IdentityRole> _roleManager, IEmailService _service, UniqloDbContext _context, IMemoryCache _cache) : Controller
    {
        private bool isAuthenticated => HttpContext.User.Identity?.IsAuthenticated ?? false;
        public IActionResult Register()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
                return View();
            User user = new User
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));
            if (!roleResult.Succeeded)
            {
                foreach (var err in roleResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            Random r = new Random();
            int code = r.Next(1000, 9999);
            //await _context.UserCodes.AddAsync(new UserCode
            //{
            //    UserId = user.Id,
            //    Code = code
            //});
            //await _context.SaveChangesAsync();
            _cache.Set(user.Id, code, DateTimeOffset.Now.AddMinutes(30));
            _service.SendEmailConfirmation(user.Email, user.UserName, code.ToString());
            return Content("Email sent!");
        }

        //public async Task<IActionResult> Method()
        //{
        //    foreach (Roles item in Enum.GetValues(typeof(Roles)))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole(item.GetRole()));
        //    }
        //    return Ok();
        //}
        public IActionResult Login()
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View();
            User? user = null;
            if (vm.UsernameOrEmail.Contains("@"))
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            else
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password is wrong!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "You must confirm your account");
                }
                return View();
            }

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                if (await _userManager.IsInRoleAsync(user,"Admin"))
                {
                    return RedirectToAction("Index", new { Controller = "Dashboard", Area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }
            return LocalRedirect(returnUrl);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> VerifyEmail(int code, string user)
        {
            var entity = await _userManager.FindByNameAsync(user);
            if (entity is null) return BadRequest();
            int? cacheCode = _cache.Get<int>(entity.Id);
            if (!cacheCode.HasValue || cacheCode != code)
                return BadRequest();
            entity.EmailConfirmed = true;
            await _userManager.UpdateAsync(entity);
            _cache.Remove(entity.Id);
            await _signInManager.SignInAsync(entity, true);
            return RedirectToAction("Index","Home");
            
        }
        public async Task<IActionResult> ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _service.SendEmailConfirmation(user.Email!, user.UserName!, token);
            return Content("Sent");
        }
        public async Task<IActionResult> NewPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(string user,string token, string pass)
        {
            var entity = await _userManager.FindByNameAsync(user);
            if (entity == null) return NotFound();
            
            var result = await _userManager.ResetPasswordAsync(entity, token.Replace(' ','+'), pass);
            return Json(result.Succeeded);
        }
    }
}
