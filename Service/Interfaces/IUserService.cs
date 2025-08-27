using Common.Models;
using Microsoft.AspNetCore.Http;
using Service.Models.User;
using static Common.Models.PaginationModel;

namespace Service.Interfaces
{
    /// <summary>
    /// Defines operations for managing users, including
    /// CRUD, pagination/filtering, and avatar management.
    /// </summary>
    public interface IUserService
    {
        #region Basic CRUD

        /// <summary>
        /// Get a single user's details by their unique ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>User details if found, otherwise null.</returns>
        UserModel? GetUserDetailById(long userId);

        /// <summary>
        /// Get details of all users.
        /// </summary>
        /// <returns>List of all users (empty list if none).</returns>
        List<UserModel> GetUserDetails();

        /// <summary>
        /// Add a new user.
        /// </summary>
        /// <param name="model">The user data to insert.</param>
        /// <returns>Newly created user ID, or null if already exists.</returns>
        long? AddUser(AddUserModel model);

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="userId">User ID to update.</param>
        /// <param name="model">New user data.</param>
        /// <returns>Response model with status and message.</returns>
        ResponseModel UpdateUser(long userId, UpdateUserModel model);

        /// <summary>
        /// Delete a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if deleted, false if user not found.</returns>
        bool DeleteUser(long userId);

        #endregion

        #region Paging / Filtering

        /// <summary>
        /// Get paginated, filtered, and sorted users.
        /// </summary>
        /// <param name="request">Paging and filter request (page, size, search, sort).</param>
        /// <returns>Paged result containing users and total count.</returns>
        PagedResult<UserModel>? GetUsersPaged(PagedRequest request);

        #endregion

        #region File / Avatar

        /// <summary>
        /// Save a new avatar for a user. Deletes old avatar if present.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="file">Uploaded avatar file.</param>
        /// <returns>
        /// Relative path (e.g., "uploads/avatars/abcd.png"),
        /// or null if user not found.
        /// </returns>
        string? UpdateAvatar(long userId, IFormFile file);

        #endregion
    }
}
