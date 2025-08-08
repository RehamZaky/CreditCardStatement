using AutoMapper;
using CreditCardStatementApi.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using WebApplicationAPI.DTO;

namespace CreditCardStatementApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<UserModel> userManager,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<UserResponse> login(UserLoginRequest userRequest)
        {
           UserModel userModel = _mapper.Map<UserLoginRequest,UserModel>(userRequest);
           var user = await _userManager.FindByEmailAsync(userModel.Email);
           var responseUser = await _userManager.CheckPasswordAsync(userModel, userRequest.Password);
            if(user == null || !responseUser)
            {
                throw new InvalidCredentialException("The email or password is incorrect");
            }

            var response = await _userManager.UpdateAsync(userModel);
            if(!response.Succeeded)
            {
                throw new Exception("Can't update user");
            }
            // login
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30";
            return userResponse;
            /// create token

        }


        public async Task<UserResponse> Register(UserRegisterRequest userRequest)
        {
            UserModel userModel = _mapper.Map<UserRegisterRequest, UserModel>(userRequest);
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if(user != null)
            {
                throw new Exception("Email already exist");
            }


            userModel.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);
            var identityResult = await _userManager.CreateAsync(userModel,userRequest.Password);
            if(!identityResult.Succeeded)
            {
                String errors = "";
                foreach (var item in identityResult.Errors)
                {
                    errors += item.Description + " ";
                    
                }        
                // return error
            }
            await _userManager.AddToRoleAsync(userModel, userRequest.Role);

            //GENERATE TOKEN
            var userResponse = _mapper.Map<UserResponse>(userModel);
            userResponse.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30";
            userResponse.Role = userRequest.Role; 
            
            return userResponse;
        }
    }
}
