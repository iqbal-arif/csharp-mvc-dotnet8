﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Retail.Models;

namespace Retail.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    // Creates DataBase Table name as "Categories"
    public DbSet<Category> Categories { get; set; }

    // Creates DataBase Table name as "Products"
    public DbSet<Product> Products { get; set; }
    
    // Creates DataBase Table name as "Company"
    public DbSet<Company> Companies { get; set; }
    
    // Creates DataBase Table name as "ShoppingCarts"
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    

    //Adding DbSet for Application Users (Custom User Properties)
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    // Creates DataBase Table name as "OrderHeaders"
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    
    // Creates DataBase Table name as "OrderDetails"
    public DbSet<OrderDetail> OrderDetails { get; set; }




    // Helper Function : ModelBuilder to seed data
    //ModelBuilder is in EF funciton used to Seed Data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Adding following Configurtion code as, Keys of Idendity tables are Mapped in the OnModelCreating
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
            new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
            new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );

        modelBuilder.Entity<Company>().HasData(
            new Company { Id = 1, Name = "Tech Solution", StreetAddress = "123 Tech St", City = "Tech City", PostalCode = "12121", State = "IL", PhoneNumber = "1235264586" },
            new Company { Id = 2, Name = "Vivid Books", StreetAddress = "999 vid St", City = "Vid City", PostalCode = "66653", State = "IL", PhoneNumber = "6328758965" },
            new Company { Id = 3, Name = "Reader Club", StreetAddress = "758 Main St", City = "Lala Land", PostalCode = "99685", State = "NY", PhoneNumber = "4529687458" }
            
            );
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Title = "Fortune of Time",
                Author = "Billy Spark",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SWD9999001",
                ListPrice = 99,
                Price = 90,
                Price50 = 85,
                Price100 = 80,
                CategoryId = 1,
                ImageUrl = ""
            },
            new Product
            {
                Id = 2,
                Title = "Dark Skies",
                Author = "Nancy Hoover",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "CAW777777701",
                ListPrice = 40,
                Price = 30,
                Price50 = 25,
                Price100 = 20,
                CategoryId = 1,
                ImageUrl = ""
            },
            new Product
            {
                Id = 3,
                Title = "Vanish in the Sunset",
                Author = "Julian Button",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "RITO5555501",
                ListPrice = 55,
                Price = 50,
                Price50 = 40,
                Price100 = 35,
                CategoryId = 1,
                ImageUrl = ""
            },
            new Product
            {
                Id = 4,
                Title = "Cotton Candy",
                Author = "Abby Muscles",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "WS3333333301",
                ListPrice = 70,
                Price = 65,
                Price50 = 60,
                Price100 = 55,
                CategoryId = 2,
                ImageUrl = ""
            },
            new Product
            {
                Id = 5,
                Title = "Rock in the Ocean",
                Author = "Ron Parker",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SOTJ1111111101",
                ListPrice = 30,
                Price = 27,
                Price50 = 25,
                Price100 = 20,
                CategoryId = 2,
                ImageUrl = ""
            },
            new Product
            {
                Id = 6,
                Title = "Leaves and Wonders",
                Author = "Laura Phantom",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "FOT000000001",
                ListPrice = 25,
                Price = 23,
                Price50 = 22,
                Price100 = 20,
                CategoryId = 3,
                ImageUrl = ""
            }
            );


    }
}
