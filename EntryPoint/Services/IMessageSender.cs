using Test.DTOs;

namespace Test.Services
{
    public interface IMessageSender
    {
        ValidationResult Send<T>(T message);
    }
}