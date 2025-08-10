using Application.DTO.Auth;
using Application.DTO.Users;
using AutoMapper;
using Domain.Entities;

namespace TCCD_Task.Mappings
{
    public class UserMapping: Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterRequest, User>().ReverseMap();
            CreateMap<LoginRequest, User>().ReverseMap();
            CreateMap<AuthResponse, User>().ReverseMap();
            CreateMap<User, UpdateUserRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();

        }
    }
  
}
