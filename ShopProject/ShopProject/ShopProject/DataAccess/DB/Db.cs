
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DB
{
    public class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "iPhone", CategoryStatus = "1" },
                new Category { CategoryId = 2, CategoryName = "SamSung", CategoryStatus = "1" },
                new Category { CategoryId = 3, CategoryName = "Nokia", CategoryStatus = "1" },
                new Category { CategoryId = 4, CategoryName = "Vivo", CategoryStatus = "1" },
                new Category { CategoryId = 5, CategoryName = "Oppo", CategoryStatus = "1" },
                new Category { CategoryId = 6, CategoryName = "Xiaomi", CategoryStatus = "1" },
                new Category { CategoryId = 7, CategoryName = "Realme", CategoryStatus = "1" },
                new Category { CategoryId = 8, CategoryName = "Masstel", CategoryStatus = "1" },
                new Category { CategoryId = 9, CategoryName = "Itel", CategoryStatus = "1" },
                new Category { CategoryId = 10, CategoryName = "Mobell", CategoryStatus = "1" }
            );
          

         }

    }
}
