using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class LinhaCompra
    {
        public int CompraId { get; set; }
        public List<int> stockIds { get; set; }
    }
}
