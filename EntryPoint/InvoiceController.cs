using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Test.Models;
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
        private readonly IIdentityService _identityService;

        public InvoiceController(IMessageSender messageSender, IRepository repository, ILogger<InvoiceController> logger, IIdentityService identityService)
        {
            _messageSender = messageSender;
            _repository = repository;
            _logger = logger;
            _identityService = identityService;
        }

        [HttpPost("add")]
        [Authorize]
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
        
        [HttpPost("login")]
        public IActionResult Token(Person person)
        {
            var identity = _identityService.GetIdentity(person.Login, person.Password);
            if (identity is null)
                return Unauthorized();

            var encodedJwt = _identityService.GetToken(identity);
 
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
 
            return Ok(response);
        }
    }
}