using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Implementation
{
    /// <summary>
    /// Handles saving and deleting uploaded files (avatars, etc) to Azure Blob Storage.
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<FileService> _logger;
        private readonly BlobContainerClient _containerClient;
        private readonly string[] _allowedExtensions;
        private readonly long _maxFileSize;

        public FileService(IConfiguration config, ILogger<FileService> logger)
        {
            _config = config;
            _logger = logger;

            var connectionString = _config["AzureBlobStorage:ConnectionString"];
            var containerName = _config["AzureBlobStorage:ContainerName"] ?? "uploads";

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);

            _allowedExtensions = _config.GetSection("Upload:AllowedExtensions").Get<string[]>()
                                 ?? new[] { ".png", ".jpg", ".jpeg", ".webp" };
            _maxFileSize = long.TryParse(_config["Upload:MaxAvatarBytes"], out var bytes) ? bytes : 5_242_880;
        }

        /// <summary>
        /// Upload file to Azure Blob Storage and return public URL.
        /// </summary>
        public string SaveAvatar(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty", nameof(file));

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                throw new InvalidOperationException($"File extension {ext} is not allowed.");

            if (file.Length > _maxFileSize)
                throw new InvalidOperationException($"File size exceeds limit ({_maxFileSize} bytes).");

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var blobName = string.IsNullOrWhiteSpace(subFolder) ? fileName : $"{subFolder.TrimEnd('/')}/{fileName}";

            var blobClient = _containerClient.GetBlobClient(blobName);
            using (var stream = file.OpenReadStream())
            {
                blobClient.Upload(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }

            _logger.LogInformation("Uploaded file {BlobName} (size: {Size})", blobName, file.Length);

            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Deletes a file from Azure Blob Storage by URL.
        /// </summary>
        public bool DeleteFile(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl)) return false;

            try
            {
                var uri = new Uri(fileUrl);
                var blobName = uri.AbsolutePath.TrimStart('/').Split('/').Skip(1).Aggregate((a, b) => $"{a}/{b}");

                var blobClient = _containerClient.GetBlobClient(blobName);
                var deleted = blobClient.DeleteIfExists();

                _logger.LogInformation(deleted ? "Deleted file {BlobName}" : "File not found {BlobName}", blobName);
                return deleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file {FileUrl}", fileUrl);
                return false;
            }
        }
    }
}
