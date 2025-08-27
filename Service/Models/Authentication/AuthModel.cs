namespace Service.Models.Authentication
{
    /// <summary>
    /// Represents a simplified authentication response model
    /// with basic user info and issued JWT token.
    /// </summary>
    public class AuthModel
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        public string Firstname { get; set; } = string.Empty;

        /// <summary>
        /// User's last name.
        /// </summary>
        public string Lastname { get; set; } = string.Empty;

        /// <summary>
        /// User's email address (unique identifier for login).
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's role (Admin, User, Supplier, etc.).
        /// 
        /// ⚠️ Suggestion: Instead of string, 
        /// consider using <see cref="Common.Enums.Roles"/> enum for type-safety.
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// JWT token issued for the user session.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
