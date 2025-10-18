namespace catalogo_service.Helpers
{
    public class QueryObject
    {
        public string? Categoria { get; set; }
        public string? Color { get; set; }
        public string? Talla { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
    }
}