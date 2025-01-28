using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using mywebsite.Data;
using mywebsite.Models.Products;
using mywebsite.Models.Search;
using mywebsite.Models.Users;




namespace mywebsite.Controllers
{
    public class ManagerController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        // Об'єднаний конструктор для всіх залежностей
        public ManagerController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckProducts()
        {
            var model = new ProductSearchViewModel(); 
            return View(model);
        }


        [HttpGet]
        public IActionResult SearchProducts(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                var model = new ProductSearchViewModel
                {
                    ProductId = productId,
                    Product = product
                };

                return View(model); // Повертаємо модель на сторінку пошуку товарів
            }
            else
            {
                // Якщо товар не знайдений, повертаємо повідомлення про помилку
                ViewBag.ErrorMessage = "Товар не знайдено!";
                return View();
            }
        }
        public IActionResult AddKeyboard()
        {
            return View("~/Views/Products/AddKeyboard.cshtml"); 
        }

        [HttpPost]
        public IActionResult AddKeyboard(Keyboard model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Модель валідна, починаємо обробку зображення та додавання продукту.");

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        Console.WriteLine("Зображення знайдено, починаємо обробку.");

                        // Викликаємо новий метод для збереження зображення
                        var fileName = model.Name; // Використовуємо назву моделі як ім'я файлу
                        model.ImagePath = SaveImage(imageFile, fileName);

                        Console.WriteLine($"Зображення успішно збережено за шляхом: {model.ImagePath}");
                    }
                    else
                    {
                        Console.WriteLine("Зображення не надано.");
                    }

                    // Додаємо продукт до бази даних
                    Console.WriteLine("Починаємо додавання продукту та клавіатури.");
                    AddProductToDatabase(model);

                    Console.WriteLine("Клавіатура успішно додана.");
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Сталася помилка: {ex.Message}");
                    Console.WriteLine($"Деталі помилки: {ex.StackTrace}");
                }
            }
            else
            {
                Console.WriteLine("Модель не валідна, перевіряємо помилки.");
                foreach (var field in ModelState)
                {
                    if (field.Value.Errors.Any())
                    {
                        foreach (var error in field.Value.Errors)
                        {
                            Console.WriteLine($"Поле: {field.Key}, Помилка: {error.ErrorMessage}");
                        }
                    }
                }
            }

            // Повертаємо ту ж форму, якщо модель не валідна
            return View(model);
        }



        private void AddProductToDatabase(object model)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var product = new Product();
                _context.Products.Add(product);
                _context.SaveChanges();
                switch (model)
                {
                    case Keyboard keyboard:
                        // Додавання клавіатури
                        keyboard.ProductId = product.Id;
                        _context.Keyboards.Add(keyboard);
                        break;

                    case Switch switchModel:
                        // Додавання перемикача
                        switchModel.ProductId = product.Id;
                        _context.Switches.Add(switchModel);
                        break;

                    case Barebone bareboneModel:
                        // Додавання бази
                        bareboneModel.ProductId = product.Id;
                        _context.Barebones.Add(bareboneModel);
                        break;
                    case Keycaps keycapsModel:
                        // Додавання кейкапів
                        keycapsModel.ProductId = product.Id;
                        _context.Keycaps.Add(keycapsModel);
                        break;

                    default:
                        throw new InvalidOperationException("Невідома модель продукту");
                }

                // Збереження змін в базі
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                // У випадку помилки відкатуємо транзакцію
                transaction.Rollback();
                throw;
            }
        }

        private string SaveImage(IFormFile imageFile, string fileName, string folderPath = "images/keyboards")
        {
            try
            {
                // Генеруємо шлях для збереження зображення
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Додаємо розширення до імені файлу, якщо його немає
                if (!Path.HasExtension(fileName))
                {
                    fileName += Path.GetExtension(imageFile.FileName); // Використовуємо оригінальне розширення
                }

                // Повний шлях до файлу
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Збереження файлу
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                // Повертаємо відносний шлях для збереження в модель
                return $"/{folderPath}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при збереженні зображення: {ex.Message}");
                throw;
            }
        }




    }

}
