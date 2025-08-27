namespace Common.Helpers
{
    /// <summary>
    /// Centralized cache key generator to avoid collisions.
    /// </summary>
    public static class CacheKeys
    {
        // ========================
        // User cache keys
        // ========================
        public const string UsersAll = "users:all";

        public static string UserById(long userId) => $"users:{userId}";

        public static string UsersPaged(int page, int size, string? search, string? sortBy, bool desc)
        {
            search = string.IsNullOrWhiteSpace(search) ? "" : search.Trim().ToLower();
            sortBy = sortBy ?? "";
            return $"users:paged:{page}_{size}_{search}_{sortBy}_{(desc ? 1 : 0)}";
        }

        // ========================
        // Role cache keys
        // ========================
        public const string RolesAll = "roles:all";
        public static string RoleById(byte roleId) => $"roles:{roleId}";

        // ========================
        // Address cache keys
        // ========================
        public static string AddressesByUser(long userId) => $"addresses:user:{userId}";
    }
}
