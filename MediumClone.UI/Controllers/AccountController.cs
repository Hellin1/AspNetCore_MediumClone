using MediumClone.Dtos.NlogDtos;
using MediumClone.Entities.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MediumClone.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View(new AppUserSignInDto());
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn(AppUserSignInDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(dto.Username);
                var signInResult = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, dto.RememberMe, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (signInResult.IsLockedOut)
                {
                    var lockOutEnd = await _userManager.GetLockoutEndDateAsync(user);

                    ModelState.AddModelError("", $"Hesabınız {(lockOutEnd.Value.UtcDateTime - DateTime.UtcNow).Minutes} dk askıya alınmıştır");
                }
                else
                {
                    var message = string.Empty;

                    if (user != null)
                    {
                        var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                        message = $"{(_userManager.Options.Lockout.MaxFailedAccessAttempts - failedCount)} kez daha denerseniz hesabınız geçici olarak kilitlenecektir";
                    }
                    else
                    {
                        message = "Kullanıcı adı veya şifre hatalı";
                    }


                    ModelState.AddModelError("", message);
                }
            }
            return View(dto);

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public IActionResult Create()
        {
            return View(new AppUserCreateDto());
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(AppUserCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Email = dto.Email,
                    UserName = dto.Username
                };

                if (!await _roleManager.RoleExistsAsync("Member"))
                {
                    var role = new AppRole { Name = "Member" };
                    var result = await _roleManager.CreateAsync(role);
                }
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    var role = new AppRole { Name = "Admin" };
                    var result = await _roleManager.CreateAsync(role);
                }

                var identityResult = await _userManager.CreateAsync(user, dto.Password);
                if (identityResult.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);
                    var result = await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(dto);
        }
    }
}
