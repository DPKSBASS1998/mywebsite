using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mywebsite.Models;
using mywebsite.Models.Users;

namespace mywebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            // Якщо користувач автентифікований
            if (User.Identity.IsAuthenticated)
            {
                // Отримуємо користувача
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    // Перевіряємо роль
                    if (await _userManager.IsInRoleAsync(user, "Manager"))
                    {
                        // Перенаправляємо менеджера на сторінку менеджера
                        return RedirectToAction("Index", "Manager");
                    }
                }
            }

            // Якщо користувач не авторизований, показуємо стандартну головну сторінку
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
