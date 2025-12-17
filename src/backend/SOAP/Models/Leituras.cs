using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOAP.Models
{
    public class Leituras
    {
        public int Id { get; set; }

        public int SensorId { get; set; }

        public float Valor { get; set; }

        public DateTime DataHora { get; set; }
    }
}