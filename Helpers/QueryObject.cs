namespace catalogo_service.Helpers
{
    public class QueryObject
    {
        public List<string>? Categoria { get; set; }
        public List<string>? Color { get; set; }
        public List<string>? Talla { get; set; }
        public List<string>? Genero { get; set; }
        public List<string>? Deporte { get; set; }
        public List<string>? Tipo { get; set; }
        public List<string>? Coleccion { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}