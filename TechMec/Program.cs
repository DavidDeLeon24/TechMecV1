using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechMec.Data;
using TechMec.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 1. Database Configuration (MS SQL Server via EF Core)
// ---------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ---------------------------------------------------------
// 2. ASP.NET Core Identity Configuration
//    Matches FR-104: 3 failed attempts -> 15 min lockout
//    Matches FR-103: 30 min inactivity timeout
//    Matches NFR-201: PBKDF2 (default in Identity)
// ---------------------------------------------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password policy
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout policy (FR-104)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // Sign-in policy
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Session / cookie expiration (FR-103: 30 min inactivity)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// ---------------------------------------------------------
// 3. MVC
// ---------------------------------------------------------
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ---------------------------------------------------------
// 4. Seed a Test User (remove in production)
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed roles (matches your stakeholder list in Section 4.1)
    string[] roles = { "Nurse", "ChargeNurse", "Supervisor", "Admin" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Seed a test nurse account
    var testEmail = "nurse@techmec.com";
    if (await userManager.FindByEmailAsync(testEmail) == null)
    {
        var testUser = new ApplicationUser
        {
            UserName = testEmail,
            Email = testEmail,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(testUser, "Nurse123!");
        if (result.Succeeded)
            await userManager.AddToRoleAsync(testUser, "Nurse");
    }
}

// ---------------------------------------------------------
// 5. Middleware Pipeline
// ---------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // Must come BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();