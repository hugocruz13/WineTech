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
        public string ImgVinho { get; set; }
        public string NomeUtilizador { get; set; }
        public string EmailUtilizador { get; set; }
        public string ImagemUtilizador { get; set; }
        public int StockId { get; set; }
        public int Cartao { get; set; }
        public string idUtilizador { get; set; }
    }
}
