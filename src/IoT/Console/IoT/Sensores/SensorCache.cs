using IoT.Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoT.Logica;

namespace IoT.Sensores
{
    public class SensorCache
    {
        public static List<SensorDTO> SensoresEmMemoria { get; set; } = new List<SensorDTO>();
        public static async Task VerificarNovosSensores(List<SensorDTO> sensoresRecebidos)
        {
            var novosSensores = sensoresRecebidos.Where(apiSensor => !SensoresEmMemoria.Any(memSensor => memSensor.Id == apiSensor.Id)).ToList();

            if (novosSensores.Count > 0)
            {
                Console.WriteLine($"Encontrados {novosSensores.Count} novos sensores!");

                SensoresEmMemoria.AddRange(novosSensores);

                foreach (var sensor in novosSensores)
                {
                    await ValueResolver.ConfigurarSensor(sensor);
                    // Console write de teste, deteta os novos sensores
                    Console.WriteLine($" -> Sensor {sensor.Id} ({sensor.IdentificadorHardware}) enviado para configuração.");
                }
            }
            else
            {
                Console.WriteLine("Nenhum sensor novo detetado");
            }
        }
    }
}
