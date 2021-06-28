using Core;
using Microsoft.EntityFrameworkCore;

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
    }
}