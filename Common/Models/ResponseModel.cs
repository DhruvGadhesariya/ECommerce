namespace Common.Models
{
    /// <summary>
    /// Standard API response wrapper.
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Optional identifier of the entity involved in the response (e.g., UserId).
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// HTTP-like status code (200 = OK, 400 = Bad Request, etc.).
        /// </summary>
        public int StatusCode { get; set; } = 200;

        /// <summary>
        /// A short human-readable message for clients.
        /// </summary>
        public string Message { get; set; } = "OK";

        /// <summary>
        /// Data payload (could be object, list, DTO, etc.).
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Optional flag indicating whether the request was successful.
        /// This helps frontends quickly check success/failure without parsing StatusCode.
        /// </summary>
        public bool Success => StatusCode >= 200 && StatusCode < 300;
    }
}
