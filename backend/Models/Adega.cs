using System.Collections.Generic;

namespace Models
{
    public class Adega
    {
        public int Id { get; set; } = 0;
        public string Nome { get; set; }
        public string Localizacao { get; set; }
        public int Capacidade { get; set; }
        public string ImagemUrl { get; set; }
        public List<Vinho> Vinhos { get; set; }

    }
}
