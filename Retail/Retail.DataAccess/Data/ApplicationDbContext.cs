using Microsoft.EntityFrameworkCore;
using Retail.Models;

namespace RetailWeb.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // Creates DataBase Table name as "Categories"
        public DbSet<Category> Categories { get; set; }

        // Helper Function : ModelBuilder to seed data
        //ModelBuilder is in EF funciton used to Seed Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }
    }
}
