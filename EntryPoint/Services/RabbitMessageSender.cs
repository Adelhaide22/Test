using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Test.Models;

namespace Test.Services
{
    public class RabbitMessageSender : IMessageSender
    {
        private readonly RabbitOptions _options;
        private readonly ILogger<RabbitMessageSender> _logger;

        public RabbitMessageSender(IOptions<RabbitOptions> options, ILogger<RabbitMessageSender> logger)
        {
            _logger = logger;
            _options = options.Value;
        }

        public ValidationResult Send<T>(T message)
        {
            try
            {
                _logger.LogInformation("Sending invoice to rabbit queue");
                var factory = new ConnectionFactory
                {
                    HostName = _options.Hostname,
                    Password = _options.Password,
                    UserName = _options.UserName,
                    VirtualHost = _options.VirtualHost
                };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(_options.Exchange, "", null, body);
                
                _logger.LogInformation("Sending state: successful");
                return new ValidationResult {IsValid = true};
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new ValidationResult {IsValid = false};
            }
        }
    }
}