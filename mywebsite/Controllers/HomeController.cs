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
            // ���� ���������� ����������������
            if (User.Identity.IsAuthenticated)
            {
                // �������� �����������
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    // ���������� ����
                    if (await _userManager.IsInRoleAsync(user, "Manager"))
                    {
                        // ��������������� ��������� �� ������� ���������
                        return RedirectToAction("Index", "Manager");
                    }
                }
            }

            // ���� ���������� �� �������������, �������� ���������� ������� �������
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
