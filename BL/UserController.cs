using BL.JWT;
using Common;
using Common.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using Models.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BL
{
    public class UserController
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserHint> _userHintRepository;
        private readonly IRepository<Hint> _hintRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserController(IRepository<User> userRepository,
            IRepository<UserHint> userHintRepository, IRepository<Hint> hintRepository,
            IOptions<JwtSettings> jwtSettings, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _userHintRepository = userHintRepository;
            _hintRepository = hintRepository;
            _jwtSettings = jwtSettings.Value;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponse> AddUserAsync(UserDto userDto)
        {
            var userAll = await _userRepository.GetAllAsync();
            var userFound = userAll.FirstOrDefault(u => u.EmailUser == userDto.EmailUser);
            if (userFound != null)
            {
                return new UserResponse(402, "A user with this email is already registered", true, null, null);
            }
            string hashedPassword = _passwordHasher.HashPassword(null, userDto.Password);
            var user = new User
            {
                UserName = userDto.UserName,
                EmailUser = userDto.EmailUser,
                Password = hashedPassword
            };

            user = await _userRepository.AddAsync(user);

            var hint = await _hintRepository.GetAllAsync();
            var hintFirst = hint.FirstOrDefault(h => h.Id == 1);
            var hintSecond = hint.FirstOrDefault(h => h.Id == 2);
            var hintThird = hint.FirstOrDefault(h => h.Id == 3);

            var userHint = new List<UserHint>
            {
                new UserHint
                {
                    UserId = user.Id,
                    HintId = hintFirst.Id,
                    IsUsed = true,
                },
                new UserHint
                {
                    UserId = user.Id,
                    HintId = hintSecond.Id,
                    IsUsed = true,
                },
                new UserHint
                {
                    UserId = user.Id,
                    HintId = hintThird.Id,
                    IsUsed = true,
                }

            };
            user.UserHints.AddRange(userHint);
            await _userHintRepository.AddAllAsync(userHint);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Surname, user.UserName.ToString())
            };

            var token = GenerateJwtToken(claims);
            return new UserResponse(200, "User is added", true, token, user);

        }

        public async Task<UserResponse> AuthorizeUserAsync(UserAuthorizeDto userAuthorizeDto)
        {
            var user = await _userRepository.GetAllAsync();
            var userFound = user.FirstOrDefault(u=>u.EmailUser== userAuthorizeDto.EmailUser);

            if (userFound == null || _passwordHasher.VerifyHashedPassword(null, userFound.Password, userAuthorizeDto.Password) != PasswordVerificationResult.Success)
            {
                return new UserResponse(401, "Wrong login or password", false, null, null);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userFound.Id.ToString()),
                new Claim(ClaimTypes.Surname, userFound.UserName.ToString())
            };

            var token = GenerateJwtToken(claims);
            return new UserResponse(200, "You authorize", true, token, userFound);
        }
        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(7);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
