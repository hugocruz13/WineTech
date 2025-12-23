using System;

namespace SOAP.Models
{
    public class AlertaComSensor
    {
        public int Id { get; set; }
        public string TipoAlerta { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataHora { get; set; }
        public bool Resolvido { get; set; }
        public int SensoresId { get; set; }
        public string IdentificadorHardware { get; set; }
        public string TipoSensor { get; set; }
        public int AdegaId { get; set; }
    }
}
