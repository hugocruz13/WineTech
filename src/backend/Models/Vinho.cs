using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Vinho
    {
        public int Id { get; set; } = 0;
        public string Nome { get; set; }
        public string Produtor { get; set; }
        public int Ano { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string ImagemUrl { get; set; }
        public float Preco { get; set; }
    }
}
