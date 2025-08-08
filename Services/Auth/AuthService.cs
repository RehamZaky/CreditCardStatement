using AutoMapper;
using CreditCardStatementApi.Model;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using WebApplicationAPI.DTO;
using WebApplicationAPI.Service.Auth;

namespace CreditCardStatementApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IMapper _mapper;
        public readonly ITokenService _tokenService;

        public AuthService(UserManager<UserModel> userManager,IMapper mapper,ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
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

            /// create token
           var token = await _tokenService.GenerateToken(user);

            var response = await _userManager.UpdateAsync(userModel);
            if(!response.Succeeded)
            {
                throw new Exception("Can't update user");
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Token = token;
            return userResponse;

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
                throw new Exception(errors);    
            }
            await _userManager.AddToRoleAsync(userModel, userRequest.Role);

            //GENERATE TOKEN
            var token = await _tokenService.GenerateToken(userModel);

            var userResponse = _mapper.Map<UserResponse>(userModel);
            userResponse.Token = token;
            userResponse.Role = userRequest.Role; 
            
            return userResponse;
        }
    }
}
