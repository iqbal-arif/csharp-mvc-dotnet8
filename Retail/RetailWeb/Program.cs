using Microsoft.EntityFrameworkCore;
using Retail.DataAccess.Repository.IRepository;
using Retail.Models;
using RetailWeb;
using Retail.DataAccess.Repository;
using Retail.DataAccess.Data;
using Microsoft.AspNetCore.Identity;


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

            builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

            //The bottom code is left to to show Email verification Option IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true) has be delete
            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); /// Implementation of CategoryRepository

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
