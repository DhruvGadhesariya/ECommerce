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

        public static string UsersPaged(int page, int size, string? search, string? sortBy, bool desc)
        {
            search = string.IsNullOrWhiteSpace(search) ? "" : search.Trim().ToLower();
            sortBy = sortBy ?? "";
            return $"users:paged:{page}_{size}_{search}_{sortBy}_{(desc ? 1 : 0)}";
        }
    }
}
