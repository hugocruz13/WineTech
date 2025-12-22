using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Leituras
    {
        public int Id { get; set; }

        public int SensorId { get; set; }

        public float Valor { get; set; }

        public DateTime DataHora { get; set; }
    }
}
