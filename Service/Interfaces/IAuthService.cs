using Service.Models.Authentication;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        public AuthModel Login(UserLoginModel model);

        public string EncryptedPassword(string? password);

        public string GetUserDetail(long UserId);

        public string Registration(RegistrationModel model);
    }
}
