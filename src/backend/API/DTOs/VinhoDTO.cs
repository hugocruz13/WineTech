namespace API.DTOs
{
    public class VinhoDTO
    {
        public int Id { get; set; } = 0;
        public string Nome { get; set; }
        public string Produtor { get; set; }
        public int Ano { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public float Preco { get; set; }
    }
    public class CreateVinhoDTO
    {
        public string Nome { get; set; }
        public string Produtor { get; set; }
        public int Ano { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public IFormFile? ImagemUrl { get; set; }
        public float Preco { get; set; }
    }
    public class UpdateVinhoDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Produtor { get; set; }
        public int Ano { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public float Preco { get; set; }
    }
}
