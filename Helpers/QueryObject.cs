namespace catalogo.Helpers
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
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}
