using Microsoft.EntityFrameworkCore;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;
using RetailWeb;
using Retail.DataAccess.Repository;
using Retail.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Retail.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace RetailWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity Core (Login, Logout)
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Access Denied Page Redirect
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            //The bottom code is left to to show Email verification Option IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true) has be delete
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); /// Implementation of CategoryRepository

            builder.Services.AddScoped<IEmailSender, EmailSender>(); /// Implementation of EmailSender

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

            //Adding User Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            //Razor Pages Mapping
            app.MapRazorPages();

            //MVC Pages Mapping
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
