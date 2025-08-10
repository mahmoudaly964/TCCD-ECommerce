using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.Category)
            //    .WithMany(c => c.Products)
            //    .HasForeignKey(p => p.CategoryId);

            //modelBuilder.Entity<CartItem>()
            //    .HasOne<Cart>()
            //    .WithMany(c => c.Items)
            //    .HasForeignKey(ci => ci.CartId);

            //modelBuilder.Entity<CartItem>()
            //    .HasOne<Product>()
            //    .WithMany()
            //    .HasForeignKey(ci => ci.ProductId);

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.Cart)
            //    .WithOne(c => c.User)
            //    .HasForeignKey<Cart>(c => c.UserId);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

    }
}
