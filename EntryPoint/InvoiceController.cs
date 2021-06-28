using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.DTOs;
using Test.Repositories;
using Test.Services;
using ILogger = Serilog.ILogger;

namespace Test
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMessageSender _messageSender;
        private readonly IRepository _repository;
        private readonly ILogger _logger;

        public InvoiceController(IMessageSender messageSender, IRepository repository, ILogger logger)
        {
            _messageSender = messageSender;
            _repository = repository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice(Invoice invoice)
        {
            _logger.Information("Receiving invoice");
            _logger.Information($"Description: {invoice.Description}");
            _logger.Information($"Supplier: {invoice.Supplier}");
            _logger.Information($"LinesCount: {invoice.Lines.Count}");
            _logger.Information($"DueDate: {invoice.DueDate}");
            
            var insertingResult = await _repository.Insert(invoice);
            if (!insertingResult.IsValid)
            {
                _logger.Error("Database error");
                return StatusCode(500);
            }
            
            var sendingResult = _messageSender.Send(invoice);
            
            if (!sendingResult.IsValid)
            {
                _logger.Error("Message transportation error");
                return StatusCode(500);
            }
            
            _logger.Information("Invoice was successfully handled");
            return Ok();
        }
    }
}