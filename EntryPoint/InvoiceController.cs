using Microsoft.AspNetCore.Mvc;
using Test.Repositories;
using Test.Services;

namespace Test
{
    public class InvoiceController : Controller
    {
        private IInvoiceRepository _repository;
        private IMessageSender _messageSender;

        public InvoiceController(IInvoiceRepository repository, IMessageSender messageSender)
        {
            _repository = repository;
            _messageSender = messageSender;
        }

        [HttpPost]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}