using AutoMapper;
using CreditCardStatementApi.Model;
using Microsoft.AspNetCore.Identity;
using WebApplicationAPI.DTO;

namespace CreditCardStatementApi.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterRequest, UserModel>().ReverseMap();
            CreateMap<UserResponse, UserModel>().ReverseMap();
            CreateMap<UserLoginRequest, UserModel>().ReverseMap();


        }
    }
}
