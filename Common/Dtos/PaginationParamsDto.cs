namespace Common.Dtos
{
    /// <summary>
    /// Common pagination request and response models.
    /// </summary>
    public class PaginationParamsDto
    {
        /// <summary>
        /// Request model for paged data queries.
        /// </summary>
        public class PagedRequest
        {
            /// <summary>
            /// Page number (1-based).
            /// </summary>
            public int Page { get; set; } = 1;

            /// <summary>
            /// Number of items per page.
            /// </summary>
            public int Size { get; set; } = 10;

            /// <summary>
            /// Optional search term (applied on supported fields).
            /// </summary>
            public string? Search { get; set; }

            /// <summary>
            /// Optional column name to sort by.
            /// </summary>
            public string? SortBy { get; set; }

            /// <summary>
            /// If true → descending order, else ascending.
            /// </summary>
            public bool Desc { get; set; } = false;
        }

        /// <summary>
        /// Generic response model for paginated data.
        /// </summary>
        public class PagedResult<T>
        {
            /// <summary>
            /// Current page number.
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// Page size (items per page).
            /// </summary>
            public int Size { get; set; }

            /// <summary>
            /// Total number of items available (before paging).
            /// </summary>
            public long Total { get; set; }

            /// <summary>
            /// Total number of pages available (calculated).
            /// </summary>
            public int TotalPages => (int)Math.Ceiling((double)Total / Size);

            /// <summary>
            /// Items in the current page.
            /// </summary>
            public List<T> Items { get; set; } = new();
        }
    }
}
