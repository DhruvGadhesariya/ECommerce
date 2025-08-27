using Common.AppSettings;
using Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Common.Helpers
{
    public class JwtHelper
    {
        private enum UserRole
        {
            Admin = 1,
            User = 2,
            Supplier = 3,
        }

        //generates the token
        public static string GenerateToken(JwtViewModel jwtSetting, TblUser model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSetting.Key);
            var claims = new ClaimsIdentity(new Claim[]
              {
                new Claim(ClaimTypes.Name,model.UserId.ToString()),
                new Claim(ClaimTypes.UserData,model.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, model.Firstname + " "+ model.Lastname),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole),value: model.Role)),

            });
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSetting.Issuer,
                Audience = jwtSetting.Audience,
                IssuedAt = DateTime.UtcNow,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
