using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Temp
    {
        public double temperatura { get; set; }
        public DateTime dataHora { get; set; }
    }

    public class Hum
    {
        public double humidade { get; set; }
        public DateTime dataHora { get; set; }
    }

    public class Lum
    {
        public int luminosidade { get; set; }
        public DateTime dataHora { get; set; }
    }

    public class LeiturasStock
    {
        public List<Temp> Temperatura { get; set; }
        public List<Hum> Humidade { get; set; }
        public List<Lum> Luminosidade { get; set; }
    }
}
