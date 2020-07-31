using Microsoft.EntityFrameworkCore;
using System;
using VinorSoft.Tymy.Service.Entities;

namespace VinorSoft.Tymy.Service
{
    public class TymyDbContext:DbContext
    {
        public TymyDbContext(DbContextOptions<TymyDbContext> options) : base(options)
        {
        }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<CustomerTypes> CustomerTypes { get; set; }
        public DbSet<Staffs> Staffs { get; set; }
        public DbSet<StaffTypes> StaffTypes { get; set; }
        public DbSet<Tables> Tables { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductGroups> ProductGroups { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetails>().HasKey(e => new { e.OrderId, e.ProductId });

            base.OnModelCreating(modelBuilder);
        }

    }
}
