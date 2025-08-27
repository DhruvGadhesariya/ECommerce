using Common.AppSettings;
using Common.Helpers;
using Data.Data;
using Data.Entities;
using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.Models.Authentication;

namespace Service.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly EcommercedbContext _db;
        private readonly JwtViewModel _options;

        public AuthService(EcommercedbContext db, IOptions<JwtViewModel> options)
        {
            _db = db;
            _options = options.Value;
        }

        //LoginMethod to check Email and password from the database :
        public AuthModel Login(UserLoginModel model)
        {
            var roles = _db.TblUserRoles.ToList();
            var user = _db.TblUsers.FirstOrDefault(c => c.Email.Equals(model.Email.ToLower()));
            if (user is null)
            {
                return null;
            }

            bool verify = BCrypt.Net.BCrypt.Verify(model.Password, user.Password); //verify the encryptedPassword and UserInputPassword
            if (!verify)
            {
                return null;
            }

            var token = JwtHelper.GenerateToken(_options, user); //generates jwtToken
            return new AuthModel
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Role = user?.RoleNavigation?.RoleName,
                Token = token,
            };
        }

        //Encrypt the normal password to encrypted format
        public string EncryptedPassword(string? password)
        {
            var EncryptPassword = PasswordEncryption.EncryptedPassword(password);
            return EncryptPassword ?? string.Empty;
        }

        //method for getting userdetail
        public string GetUserDetail(long UserId)
        {
            var User = _db.TblUsers.Find(UserId);
            if (User != null)
            {
                return User.Firstname + " " + User.Lastname;
            }
            else
            {
                return null;
            }
        }

        //method for user registration
        public string Registration(RegistrationModel model)
        {
            var user = _db.TblUsers.Any(x => x.Email.Equals(model.Email.ToLower()));
            if (user == true)
            {
                return null;

            }
            var newuser = new TblUser();
            {
                newuser.Firstname = model.Firstname;
                newuser.Lastname = model.Lastname;
                newuser.Email = model.Email;
                newuser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                newuser.PhoneNumber = model.PhoneNumber;
                newuser.Avatar = UserAvatar.AvatarUpload(model.Avatar);
                newuser.Role = model.Role;
                newuser.CountryId = model.CountryId;
                newuser.CityId = model.CityId;
            }

            _db.TblUsers.Add(newuser);
            _db.SaveChanges();

            return "You Have Successfully Registered Yourself.You Can Login Now.";
        }

    }
}
