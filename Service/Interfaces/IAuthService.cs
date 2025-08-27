using Service.Models.Authentication;

namespace Service.Interfaces
{
    /// <summary>
    /// Defines authentication-related operations such as login, registration, and password hashing.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate a user using email & password.
        /// </summary>
        /// <param name="request">Login request (email + password).</param>
        /// <returns>
        /// AuthResponse with JWT token, expiry, and user details if successful; 
        /// null if credentials are invalid.
        /// </returns>
        AuthResponse? Login(LoginRequest request);

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="request">Registration request (firstname, lastname, email, password).</param>
        /// <returns>
        /// Newly created user ID, or null if email already exists.
        /// </returns>
        long? Register(RegisterRequest request);

        /// <summary>
        /// Generate a secure hash for a plain-text password.
        /// </summary>
        /// <param name="plain">Plain text password.</param>
        /// <returns>Hashed password string.</returns>
        string Hash(string plain);

        /// <summary>
        /// Verify if a plain-text password matches a stored hash.
        /// </summary>
        /// <param name="hash">Stored hashed password.</param>
        /// <param name="plain">Plain text password.</param>
        /// <returns>True if valid, false otherwise.</returns>
        bool Verify(string hash, string plain);
    }
}
