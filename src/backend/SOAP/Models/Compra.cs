using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOAP.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public string UtilizadorId { get; set; }
        public string Estado { get; set; }
        public double ValorTotal { get; set; }
        public int cartao { get; set; }
        public DateTime DataCompra { get; set; }
    }
}