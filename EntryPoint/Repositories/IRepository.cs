using System.Threading.Tasks;
using Test.DTOs;

namespace Test.Repositories
{
    public interface IRepository
    {
        Task<ValidationResult> Insert(Invoice invoice);
    }
}