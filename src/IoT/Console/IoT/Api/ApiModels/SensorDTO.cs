using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Api.ApiModels
{
    public class SensorDTO
    {
        public int Id { get; set; }

        public string IdentificadorHardware { get; set; }

        public string Tipo { get; set; }

        public bool Estado { get; set; }

        public int AdegaId { get; set; }
    }
    public enum TipoHardware
    {
        REAL_DHT,
        REAL_LDR,
        GERADO,
        DESCONHECIDO 
    }
}
