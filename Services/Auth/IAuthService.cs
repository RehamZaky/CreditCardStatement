using CreditCardStatementApi.Model;
using WebApplicationAPI.DTO;

namespace CreditCardStatementApi.Services.Auth
{
    public interface IAuthService
    {
        // login
        Task<UserResponse> login(UserLoginRequest userRequest);

        Task<UserResponse> Register (UserRegisterRequest userRequest);
            

            
    }
}
