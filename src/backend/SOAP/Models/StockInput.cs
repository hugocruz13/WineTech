using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOAP.Repository
{
    public class StockInput
    {
        public int VinhoId { get; set; }
        public int AdegaId { get; set; }
        public int Quantidade { get; set; }
    }
}