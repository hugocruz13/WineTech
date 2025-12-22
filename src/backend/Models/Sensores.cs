using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Sensores
    {
        public int Id { get; set; }

        public string IdentificadorHardware { get; set; }

        public string Tipo { get; set; }

        public bool Estado { get; set; }

        public string ImagemUrl { get; set; }

        public int AdegaId { get; set; }
    }
}
