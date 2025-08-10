using Application.DTO.Auth;
using Application.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services_Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterUserAsync(RegisterRequest registerRequest);
        Task<AuthResponse> LoginUserAsync(LoginRequest loginRequest);
        Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task<UserResponse> GetUserDetailsAsync(Guid userId);
    }

}
