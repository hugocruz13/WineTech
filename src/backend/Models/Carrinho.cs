using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Carrinho
    {
        public int Id { get; set; }

        public int VinhosId { get; set; }

        public string UtilizadoresId { get; set; }

        public int Quantidade { get; set; }
    }
}
