using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mywebsite.Models.Users;

namespace mywebsite.Controllers
{
    public class ManagerController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public ManagerController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
