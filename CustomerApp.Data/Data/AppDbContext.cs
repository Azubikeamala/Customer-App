using Microsoft.EntityFrameworkCore;
using CustomerApp.Domain.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CustomerApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<PaymentTerms> PaymentTerms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentTerms>().HasData(
                new PaymentTerms { Id = 1, Label = "Net 30", DueDays = 30 },
                new PaymentTerms { Id = 2, Label = "Net 60", DueDays = 60 }
            );
        }
    }
}
