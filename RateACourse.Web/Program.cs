using RateACourse.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RateACourse.Core.Entities;

namespace oe_h05_EFCore_RateAMovie_opl_Afst
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Add entity framework database
            builder.Services.AddDbContext<ApplicationDbContext>(
                 options => options.UseSqlServer(builder.Configuration.GetConnectionString("CourseRateDb")));
            //Add identity service
            builder.Services.AddIdentity<Student, IdentityRole>
                (options => 
                {
                    //configure authentication parameters
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 5;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //set identity applicationCookies
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/account/login";
                options.AccessDeniedPath = "/account/account/AccesDenied";
                options.LogoutPath = "/account/account/Logout";
            });
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //activates the logged in User features

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Account}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}