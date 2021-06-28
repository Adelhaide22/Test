using System.Security.Claims;

namespace Test.Services
{
    public interface IIdentityService
    {
        ClaimsIdentity GetIdentity(string username, string password);
        string GetToken(ClaimsIdentity identity);
    }
}