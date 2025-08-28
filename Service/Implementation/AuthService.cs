using Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using Service.Dtos.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Implementation
{
    /// <summary>
    /// Authentication service for handling user registration, login, 
    /// password hashing, and JWT token creation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserHubDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(UserHubDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        /// <summary>
        /// Registers a new user (if email does not already exist).
        /// Returns new UserId or null if duplicate.
        /// </summary>
        public long? Register(RegisterRequest req)
        {
            var email = req.Email.Trim().ToLower();

            if (_db.Users.Any(x => x.Email == email && x.DeletedAt == null))
                return null;

            var entity = new Data.Entities.User
            {
                Firstname = req.Firstname.Trim(),
                Lastname = req.Lastname.Trim(),
                Email = email,
                Password = Hash(req.Password),
                Role = 1,
                Status = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(entity);
            _db.SaveChanges();
            return entity.UserId;
        }

        /// <summary>
        /// Attempts login with email & password.
        /// Returns JWT AuthResponse or null if invalid.
        /// </summary>
        public AuthResponseDto? Login(LoginRequestDto request)
        {
            var email = request.Email.Trim().ToLower();

            var user = _db.Users.FirstOrDefault(x => x.Email == email && x.DeletedAt == null);
            if (user == null) return null;

            if (!Verify(user.Password, request.Password)) return null;

            var jwt = _config.GetSection("Jwt");
            var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new InvalidOperationException("JWT key missing"));
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, (user.Role ?? 1).ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(int.TryParse(jwt["AccessTokenMinutes"], out var mins) ? mins : 60);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires,
                UserId = user.UserId,
                FullName = $"{user.Firstname} {user.Lastname}",
                Role = user.Role
            };
        }

        /// <summary>
        /// Hash password using BCrypt.
        /// </summary>
        public string Hash(string plain) => BCrypt.Net.BCrypt.HashPassword(plain);

        /// <summary>
        /// Verify password against hash using BCrypt.
        /// </summary>
        public bool Verify(string hash, string plain) => BCrypt.Net.BCrypt.Verify(plain, hash);
    }
}
