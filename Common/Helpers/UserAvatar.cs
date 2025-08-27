using Microsoft.AspNetCore.Http;

namespace Common.Helpers
{
    public class UserAvatar
    {
        public static string AvatarUpload(IFormFile formFile)
        {
            using (var stream = formFile.OpenReadStream())
            {
                var bytes = new byte[formFile.Length];
                stream?.Read(bytes, 0, (int)formFile.Length);
                var base64string = Convert.ToBase64String(bytes);
                return "data:image/png;base64," + base64string;
            }
        }
    }
}
