namespace Service.Dtos.User
{
    /// <summary>
    /// Response model for user avatar upload.
    /// Contains both the stored relative path and a full URL for client access.
    /// </summary>
    public class UploadAvatarResponseDto
    {
        /// <summary>
        /// Path relative to the server's storage (e.g., "uploads/avatars/user1.png").
        /// </summary>
        public string RelativePath { get; set; } = string.Empty;

        /// <summary>
        /// Fully qualified public URL for accessing the avatar.
        /// Example: https://yourdomain.com/uploads/avatars/user1.png
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}
