using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class CompraDetalhe
    {
        public int IdCompra { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataCompra { get; set; }
        public int IdVinho { get; set; } = 0;
        public string Nome { get; set; }
        public string Produtor { get; set; }
        public int Ano { get; set; }
        public string Tipo { get; set; }
        public float Preco { get; set; }
        public int Quantidade { get; set; }
    }
}
