using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOAP.Models
{
    public class LinhaCompra
    {
        public int CompraId { get; set; }
        public List<int> stockIds { get; set; }

    }
}