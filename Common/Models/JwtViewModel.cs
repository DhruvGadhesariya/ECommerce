namespace Common.Models
{
    /// <summary>
    /// Holds JWT configuration values, usually mapped from appsettings.json (Jwt section).
    /// </summary>
    public class JwtViewModel
    {
        /// <summary>
        /// Secret key used to sign the JWT token (must be kept safe).
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer (the authority that creates the token).
        /// Example: your API domain.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience (the consumers of the token).
        /// Example: client apps or services.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration time in minutes (default: 60).
        /// </summary>
        public int AccessTokenMinutes { get; set; } = 60;
    }
}
