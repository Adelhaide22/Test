using System.Threading.Tasks;
using Core;
using Test.Models;

namespace Test.Repositories
{
    public interface IRepository
    {
        Task<ValidationResult> Insert(Invoice invoice);
    }
}