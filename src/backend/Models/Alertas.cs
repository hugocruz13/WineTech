using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Alertas
    {
        public int Id { get; set; }

        public int SensoresId { get; set; }

        public string TipoAlerta { get; set; }

        public string Mensagem { get; set; }

        public DateTime DataHora { get; set; }

        public bool Resolvido { get; set; }
    }
}
