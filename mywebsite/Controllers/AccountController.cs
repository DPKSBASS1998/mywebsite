using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using mywebsite.Models.Users;
using System.Threading.Tasks;

namespace mywebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Створення нового користувача
                    var user = new User
                    {
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber
                    };

                    // Перевіряємо, чи користувач увійшов у систему
                    if (User.Identity.IsAuthenticated)
                    {
                        // Перевіряємо, чи користувач має роль менеджера
                        if (User.IsInRole("Manager"))
                        {
                            Console.WriteLine($"Менеджер {User.Identity.Name} створює нового менеджера: {model.Email}");

                            // Створення менеджера
                            var result = await _userManager.CreateAsync(user, model.Password);

                            if (result.Succeeded)
                            {
                                // Призначаємо роль "Manager" новому користувачу
                                var roleResult = await _userManager.AddToRoleAsync(user, "Manager");

                                if (roleResult.Succeeded)
                                {
                                    Console.WriteLine($"Роль 'Manager' успішно призначено для {model.Email}");

                                    // Перенаправляємо на Manager Index
                                    return RedirectToAction("Index", "Manager");
                                }
                                else
                                {
                                    Console.WriteLine($"Не вдалося призначити роль 'Manager' для {model.Email}");
                                    ModelState.AddModelError(string.Empty, "Не вдалося призначити роль 'Manager'.");
                                }
                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    Console.WriteLine($"Помилка реєстрації: {error.Description}");
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            }
                        }
                        else
                        {
                            return Forbid(); // HTTP 403: Заборонено
                        }
                    }
                    else
                    {
                        // Реєстрація звичайного користувача
                        Console.WriteLine($"Створення звичайного користувача: {model.Email}");
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            // Призначаємо роль "User" новому користувачу
                            var roleResult = await _userManager.AddToRoleAsync(user, "User");

                            if (roleResult.Succeeded)
                            {
                                Console.WriteLine($"Роль 'User' успішно призначено для {model.Email}");

                                // Виконуємо вхід
                                await _signInManager.SignInAsync(user, isPersistent: false);

                                // Перенаправляємо на Home Index
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                Console.WriteLine($"Не вдалося призначити роль 'User' для {model.Email}");
                                ModelState.AddModelError(string.Empty, "Не вдалося призначити роль 'User'.");
                            }
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"Помилка реєстрації: {error.Description}");
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Модель не валідна!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Виникла помилка: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Сталася непередбачена помилка.");
            }

            // Якщо модель не валідна, повертаємо користувача на форму з помилками
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Отримуємо користувача за його електронною поштою
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Користувач не знайдений.");
                    return View(model); // Повертаємо ту саму сторінку з помилкою
                }

                // Спроба входу користувача
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // Перевірка ролі користувача
                    if (await _userManager.IsInRoleAsync(user, "Manager"))
                    {
                        return RedirectToAction("Index", "Manager");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "User"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await _signInManager.SignOutAsync();
                        ModelState.AddModelError(string.Empty, "У вас немає доступу до системи.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Метод для виходу користувача
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  // Виконання виходу

            // Після виходу перенаправляємо на головну сторінку або на сторінку входу
            return RedirectToAction("Index", "Home");
        }

    }
}
