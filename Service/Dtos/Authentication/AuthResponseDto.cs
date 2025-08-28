using Common.Dtos;

namespace Service.Dtos.Authentication
{
    /// <summary>
    /// Authentication response model.
    /// Inherits from <see cref="ApiResponseDto"/> for consistent API responses.
    /// Contains JWT token details and user info.
    /// </summary>
    public class AuthResponseDto : ApiResponseDto
    {
        /// <summary>
        /// JWT bearer token issued after successful authentication.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Expiry date and time of the issued token (UTC).
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Authenticated user's unique identifier.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Full name of the authenticated user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Role of the authenticated user.
        /// Should map to <see cref="Common.Enums.Roles"/>.
        /// </summary>
        public byte? Role { get; set; }
    }
}
