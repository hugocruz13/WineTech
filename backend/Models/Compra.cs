using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Compra
    {
        public int Id { get; set; }
        public string UtilizadorId { get; set; }
        public double ValorTotal { get; set; }
        public DateTime DataCompra { get; set; }
    }
}
