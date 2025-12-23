using IoT.Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoT.Api;
namespace IoT.Simulation
{
    public class SimulatedValueGenerator
    {
        private static readonly Random _random = new Random();
        public static void ConfigurarSensorGerado(int sensorId, string tipo, int adegaId)
        {
            TipoSensor tipoEnum = (TipoSensor)Enum.Parse(typeof(TipoSensor), tipo, true);

            double valor = 0;

            switch (tipoEnum)
            {
                case TipoSensor.Temperatura:
                    // entre 15.0 e 18.0
                    valor = 15 + (_random.NextDouble() * 3);
                    break;

                case TipoSensor.Humidade:
                    // entre 60% e 80%
                    valor = 60 + (_random.NextDouble() * 20);
                    break;

                case TipoSensor.Luminosidade:
                    // entre 0 e 200
                    valor = _random.Next(0, 201);
                    break;
                     
                default:
                    valor = 0;
                    break;
            }

            Console.WriteLine($" -> [GERADO] Tipo: {tipoEnum} | Valor Calculado: {valor}");
            var leitura = new LeituraDTO{ SensorId = sensorId, Valor = (float)Math.Round(valor, 2), Tipo = tipoEnum.ToString(), AdegaId = adegaId };

            try
            {
                ApiClient.InserirLeitura(leitura);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar para a API: " + ex.Message);
            }

        }

    }
}
