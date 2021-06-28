using Microsoft.EntityFrameworkCore;
using Test.DTOs;

namespace Test.Repositories
{
    public sealed class InvoiceContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
     
        public InvoiceContext(DbContextOptions<InvoiceContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("");
        }
    }
}