namespace API.DTOs
{
    public class AdegaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int Capacidade { get; set; }
    }

    public class CreateAdegaDTO
    {
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int Capacidade { get; set; }
        public IFormFile? Imagem { get; set; }
    }

    public class UpdateAdegaDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Localizacao { get; set; }
        public int Capacidade { get; set; } = 0;
    }

    public class AdegaStockDTO
    {
        public int VinhoId { get; set; }
        public int AdegaId { get; set; }
        public int Quantidade { get; set; }
    }
}
