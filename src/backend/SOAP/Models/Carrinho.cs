using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOAP.Models
{
    public class Carrinho
    {
        public int Id { get; set; }

        public int VinhosId { get; set; }

        public int UtilizadoresId { get; set; }

        public int Quantidade { get; set; }
    }

}