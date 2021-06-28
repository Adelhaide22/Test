using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "snake.rmq2.cloudamqp.com",
                UserName = "jyfokqut",
                Password = "smY5-UEQXGWsqsawhk5_-cjdM3vT8zHB",
                VirtualHost = "jyfokqut",
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => 
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var invoice = JsonSerializer.Deserialize<Invoice>(message);
                Console.WriteLine("Received invoice");
                Console.WriteLine($"{invoice.Description}");
                Console.WriteLine($"{invoice.Supplier}");
                Console.WriteLine($"{invoice.DueDate}");
                foreach (var line in invoice.Lines)
                {
                    Console.WriteLine($"{line.Description}");
                    Console.WriteLine($"{line.Price}");
                    Console.WriteLine($"{line.Quantity}");
                }
            };
            channel.BasicConsume("Queue#1", true, consumer);

            Console.ReadLine();
        }
    }
}