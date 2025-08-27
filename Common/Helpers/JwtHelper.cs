using Common.Models;
using Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.Helpers
{
    public static class JwtHelper
    {
        /// <summary>
        /// Generates a signed JWT token for the given user.
        /// </summary>
        public static string GenerateToken(JwtViewModel jwtSetting, User model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSetting.Key);

            // ✅ Define claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()), // unique ID
                new Claim(ClaimTypes.Name, $"{model.Firstname} {model.Lastname}"), // full name
                new Claim(ClaimTypes.Email, model.Email), // email
                new Claim(ClaimTypes.Role, model.Role?.ToString() ?? "1") // role as string
            };

            // ✅ Build token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSetting.AccessTokenMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSetting.Issuer,
                Audience = jwtSetting.Audience,
                IssuedAt = DateTime.UtcNow
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
