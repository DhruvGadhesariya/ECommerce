using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Implementation
{
    /// <summary>
    /// Handles saving and deleting uploaded files (avatars, etc).
    /// Uses configuration values for root folder, size, and allowed extensions.
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FileService> _logger;
        private readonly string _root;

        public FileService(IConfiguration config, ILogger<FileService> logger)
        {
            _config = config;
            _logger = logger;

            // Root folder (default = "wwwroot")
            _root = _config["Upload:Root"] ?? "wwwroot";
        }

        /// <summary>
        /// Save avatar file to disk and return relative path (e.g., "uploads/avatars/abc.png").
        /// </summary>
        public string SaveAvatar(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty", nameof(file));

            // Allowed extensions (default: png, jpg, jpeg, webp)
            var allowed = _config.GetSection("Upload:AllowedExtensions").Get<string[]>()
                          ?? new[] { ".png", ".jpg", ".jpeg", ".webp" };

            // Max file size (default: 5 MB)
            var maxBytes = long.TryParse(_config["Upload:MaxAvatarBytes"], out var bytes)
                           ? bytes : 5_242_880;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowed.Contains(ext))
                throw new InvalidOperationException($"File extension {ext} is not allowed.");

            if (file.Length > maxBytes)
                throw new InvalidOperationException($"File size exceeds limit ({maxBytes} bytes).");

            // Ensure folder exists
            var folder = Path.Combine(_root, subFolder);
            Directory.CreateDirectory(folder);

            // Unique filename
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            // Save file
            using (var fs = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            // Normalize relative path for URLs
            var relative = Path.Combine(subFolder, fileName).Replace('\\', '/');
            _logger.LogInformation("Saved file {Relative} (size: {Size})", relative, file.Length);

            return relative;
        }

        /// <summary>
        /// Deletes a file by relative path (returns true if deleted successfully).
        /// </summary>
        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return false;

            var full = Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(full))
            {
                try
                {
                    File.Delete(full);
                    _logger.LogInformation("Deleted file {Path}", full);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting file {Path}", full);
                    return false;
                }
            }

            _logger.LogWarning("File not found for deletion: {Path}", full);
            return false;
        }
    }
}
