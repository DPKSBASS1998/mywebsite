using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mywebsite.Models.Users;
using mywebsite.Data;

var builder = WebApplication.CreateBuilder(args);

// Налаштування сервісів
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ініціалізація ролей та користувача-менеджера
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await InitializeRoles(services);  // Ініціалізуємо ролі
}

// Налаштування HTTP запитів
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();  // Для підтримки статичних файлів

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Метод для створення ролей
async Task InitializeRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Manager", "User" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }
    }
}


