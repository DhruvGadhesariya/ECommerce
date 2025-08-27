using Microsoft.AspNetCore.Http;

namespace Service.Models.Authentication
{
    public class UserAvatarModel
    {
        public long UserId { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
