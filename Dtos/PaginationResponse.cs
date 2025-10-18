namespace catalogo.Dtos
{
    public class PaginationResponse<T> where T : class
    {
        public IEnumerable<T> Data { get; set; } = [];

        public int Total { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)Total / ItemsPerPage);
    }
}
