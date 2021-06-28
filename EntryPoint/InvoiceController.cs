using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.Repositories;
using Test.Services;

namespace Test
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IMessageSender _messageSender;
        private readonly IRepository _repository;
        private readonly ILogger<InvoiceController> _logger;

        public InvoiceController(IMessageSender messageSender, IRepository repository, ILogger<InvoiceController> logger)
        {
            _messageSender = messageSender;
            _repository = repository;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddInvoice(Invoice invoice)
        {
            _logger.LogInformation("Receiving invoice");
            _logger.LogInformation($"Description: {invoice.Description}");
            _logger.LogInformation($"Supplier: {invoice.Supplier}");
            _logger.LogInformation($"LinesCount: {invoice.Lines.Count}");
            _logger.LogInformation($"DueDate: {invoice.DueDate}");
            
            var insertingResult = await _repository.Insert(invoice);
            if (!insertingResult.IsValid)
            {
                _logger.LogError("Database error");
                return StatusCode(500);
            }
            
            var sendingResult = _messageSender.Send(invoice);
            
            if (!sendingResult.IsValid)
            {
                _logger.LogError("Message transportation error");
                return StatusCode(500);
            }
            
            _logger.LogInformation("Invoice was successfully handled");
            return Ok();
        }
    }
}