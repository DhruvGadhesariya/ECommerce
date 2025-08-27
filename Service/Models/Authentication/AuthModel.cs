namespace Service.Models.Authentication
{
    public class AuthModel
    {
        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public string Token { get; set; } = string.Empty;
    }
}
