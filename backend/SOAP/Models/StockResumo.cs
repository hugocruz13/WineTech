using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOAP.Models
{
    public class StockResumo
    {
        public int VinhoId { get; set; }  
        public string Nome { get; set; }
        public string Produtor { get; set; }
        public int Ano { get; set; }
        public string Tipo { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
    }
}