using Application.DTO.Auth;
using Application.DTO.Users;
using Application.Services_Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService( IUserRepository userRepository, IMapper mapper,IJwtService jwtService,IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponse> RegisterUserAsync(RegisterRequest request)
        {
          
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("this user already exists");

            var user = _mapper.Map<User>(request);
            user.Id = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _jwtService.GenerateAccessToken(user);

            return response;
        }

        public async Task<AuthResponse> LoginUserAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _jwtService.GenerateAccessToken(user);

            return response;
        }

        public async Task<UserResponse> GetUserDetailsAsync(Guid userId)
        {
            var user = await _userRepository.GetAsync(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            return _mapper.Map<UserResponse>(user);
        }
        public async Task<UserResponse> UpdateUserAsync(Guid userId, UpdateUserRequest request)
        {
            var user = await _userRepository.GetAsync(u => u.Id == userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            _mapper.Map(request, user);

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserResponse>(user);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
