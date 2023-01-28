using Bootstrap_3.Models;

using Bootstrap_3.ViewModels;
//using Bootstrap_3.Models;
//using Bootstrap_3.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;

namespace Anyar.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<AppUser> _userManager { get; set; }
        public SignInManager<AppUser> _signManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signManager = signManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(loginVM.UsernameorEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UsernameorEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "ur username or password invalid");
                    return View();
                }
            }

            var result = await _signManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistance, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "ur username or password invalid");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            if (!ModelState.IsValid) { return View(); }
            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
