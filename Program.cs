using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SweetWeeding.DAL;
using SweetWeeding.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequireDigit=true;
    options.Password.RequireLowercase=true;
    options.Password.RequireUppercase=true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail=true;
    options.Lockout.MaxFailedAccessAttempts=5;
    options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5); 
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = $"/Admin/Account/Login/{cfg.ReturnUrlParameter}";
});

var app = builder.Build();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();


app.MapControllerRoute(
    "Default",
    "{area:exists}/{controller=home}/{action=index}/{id?}"
    );



app.MapControllerRoute(
    "Default",
    "{controller=home}/{action=index}/{id?}"
    );

app.Run();
