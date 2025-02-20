using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mywebsite.Models.Users;
using mywebsite.Data;

var builder = WebApplication.CreateBuilder(args);

// ������������ ���� �����
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ����������� ����� �� ������������
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await InitializeRoles(services);
    await InitializeAdminUser(services);  // ��������� ������������
}

// ������������ HTTP-������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();  // ������ ��������������
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ����� ��� ��������� �����
async Task InitializeRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Manager", "User" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

async Task InitializeAdminUser(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    string adminEmail = "admin@example.com";
    string adminPassword = "Admin123!";  //  ������ �� ���� ������� ����� ��������-�������� 

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new User
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",  // ������ �������� FirstName
            LastName = "User",    // ������ �������� LastName
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Manager");
        }
    }
}

