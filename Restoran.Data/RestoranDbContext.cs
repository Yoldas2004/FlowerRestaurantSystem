using Microsoft.EntityFrameworkCore;
using Restoran.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restoran.Data
{
   public class RestoranDbContext: DbContext
    {
        public RestoranDbContext(DbContextOptions<RestoranDbContext> options) : base(options)
        {
           
        }
          
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasOne(x => x.Table)
                .WithMany()
                .HasForeignKey(x => x.TableId)
                .OnDelete(DeleteBehavior.Restrict);
            //=================================================================================
            modelBuilder.Entity<Order>().HasOne(x => x.Waiter)
                .WithMany()
                .HasForeignKey(x => x.WaiterId)
                .OnDelete(DeleteBehavior.Restrict);
            //=================================================================================
            modelBuilder.Entity<OrderItem>().HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);





        }

    }
}
