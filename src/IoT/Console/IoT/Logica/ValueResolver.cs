using IoT.Api.ApiModels;
using IoT.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Logica
{
    public class ValueResolver
    {
        public static Task ConfigurarSensor(SensorDTO sensor)
        {
            TipoHardware hardwareEnum = (TipoHardware)Enum.Parse(typeof(TipoHardware), sensor.IdentificadorHardware, true);
            switch (hardwareEnum)
            {
                case TipoHardware.REAL_DHT:
                case TipoHardware.REAL_LDR:
                    //ConfigurarHardwareReal(sensor);
                    break;

                case TipoHardware.GERADO:
                    SimulatedValueGenerator.ConfigurarSensorGerado(sensor.Id, sensor.Tipo);
                    break;

                default:
                    Console.WriteLine($"Hardware não identificado: {sensor.IdentificadorHardware}");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
