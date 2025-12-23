using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Api.ApiModels
{
    public class LeituraDTO
    {
        public int SensorId { get; set; }

        public float Valor { get; set; }
        public string Tipo { get; set; }
        public int AdegaId { get; set; }
    }
}
