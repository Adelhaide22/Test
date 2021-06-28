using System;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Logging;
using Test.Models;

namespace Test.Repositories
{
    public class Repository : IRepository
    {
        private readonly InvoiceContext _context;
        private readonly ILogger<InvoiceContext> _logger;

        public Repository(InvoiceContext context, ILogger<InvoiceContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ValidationResult> Insert(Invoice invoice)
        {
            try
            {
                _logger.LogInformation("Saving invoice to database");
                
                await _context.Invoices.AddAsync(invoice);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Saving state: successful");
                return new ValidationResult {IsValid = true};
            }
            catch (Exception e)
            {
                return new ValidationResult {IsValid = false};
            }
        }
    }
}