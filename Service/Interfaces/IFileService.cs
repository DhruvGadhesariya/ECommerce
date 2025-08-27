using Microsoft.AspNetCore.Http;

namespace Service.Interfaces
{
    /// <summary>
    /// Defines file storage operations for handling avatars and other uploads.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Saves an uploaded avatar file into a given subfolder.
        /// </summary>
        /// <param name="file">The uploaded file (IFormFile).</param>
        /// <param name="subFolder">
        /// Target subfolder inside the storage root (e.g., "avatars", "documents").
        /// </param>
        /// <returns>
        /// Relative path to the saved file (e.g., "uploads/avatars/abcd.png").
        /// </returns>
        string SaveAvatar(IFormFile file, string subFolder);

        /// <summary>
        /// Deletes a file from storage by its relative path.
        /// </summary>
        /// <param name="relativePath">Relative file path (e.g., "uploads/avatars/abcd.png").</param>
        /// <returns>True if deleted, false if file not found.</returns>
        bool DeleteFile(string relativePath);
    }
}
