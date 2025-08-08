using CreditCardStatementApi.Model;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationAPI.Service.Auth
{
    public interface ITokenService
    {
        Task<string> GenerateToken(UserModel user);
    }
}
