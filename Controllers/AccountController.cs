using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR1.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalR1.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<IdentityUser> SignInManager;
        private UserManager<IdentityUser> UserManager;
        public AccountController(SignInManager<IdentityUser> SignInManager, UserManager<IdentityUser> UserManager)
        {
            this.SignInManager = SignInManager;
            this.UserManager = UserManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
       public async Task<IActionResult> Login(string username, string password)
        {
            IdentityUser user =  await UserManager.FindByNameAsync(username);
            var result =  await SignInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                return Json(result);
            }
            return Json(new {});

        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            IdentityUser appUser = new IdentityUser()
            {
                UserName = username,
                PasswordHash = password
            };
            var result = await UserManager.CreateAsync(appUser);
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(appUser, false);
            }
            return Json(result);
        }


        public async Task<IActionResult> Logout()
        {
                await SignInManager.SignOutAsync();
            return Json(new { noti = "da logout" });
        }

        [HttpGet]
        public string Hello(string hello)
        {
            return hello;
        }
    }
}
