using AB108Uniqlo.DataAccess;
using AB108Uniqlo.Extensions;
using AB108Uniqlo.Helpers;
using AB108Uniqlo.Models;
using AB108Uniqlo.Services.Abstracts;
using AB108Uniqlo.Services.Implemets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AB108Uniqlo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<UniqloDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSql"));
            });
            builder.Services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Lockout.MaxFailedAccessAttempts = 3;
                //opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(int.MaxValue);
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<UniqloDbContext>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddMemoryCache();
            var opt = new SmtpOptions();
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection(SmtpOptions.Name));
            //builder.Services.AddSession();
            builder.Services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = "/login";
                x.AccessDeniedPath = "/Home/AccessDenied";
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseUserSeed();
            //app.UseSession();
            app.MapControllerRoute(
                name: "login",
                pattern: "login", new
                {
                    Controller = "Account",
                    Action = "Login"
                });
            app.MapControllerRoute(
                name: "register",
                pattern: "register", new
                {
                    Controller = "Account",
                    Action = "Register"
                });
            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
