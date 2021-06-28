using Test.Models;

namespace Test.Services
{
    public interface IMessageSender
    {
        ValidationResult Send<T>(T message);
    }
}