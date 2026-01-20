using BnsBazarApp.Models.Data;
using BnsBazarApp.Models.Repositories;
using BnsBazarApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------
// MVC
// ------------------------------
builder.Services.AddControllersWithViews();

// ------------------------------
// DATABASE (SQL Server)
// ------------------------------
builder.Services.AddDbContext<BnsBazarDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")
    )
);
builder.Services.AddScoped<EmailService>();

// ------------------------------
// AUTHENTICATION (CLAIMS + COOKIE)
// ------------------------------
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/Login";
        options.LogoutPath = "/Authentication/Logoff";
        options.AccessDeniedPath = "/Authentication/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

builder.Services.AddAuthorization();

// ------------------------------
// OPTIONAL: SESSION (KEEP ONLY IF USED ELSEWHERE)
// ------------------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// ------------------------------
// DI – REPOSITORIES
// ------------------------------
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

// ------------------------------
// FILE UPLOAD LIMIT (VERY LARGE)
// ------------------------------
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

var app = builder.Build();

// ------------------------------
// PIPELINE
// ------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ?? AUTH MUST COME BEFORE AUTHORIZATION
app.UseAuthentication();
app.UseAuthorization();

// ?? Session AFTER auth (only if still used)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();