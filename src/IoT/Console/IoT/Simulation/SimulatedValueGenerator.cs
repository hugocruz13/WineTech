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
        public static void ConfigurarSensorGerado(int sensorId, string tipo)
        {
            TipoSensor tipoEnum = (TipoSensor)Enum.Parse(typeof(TipoSensor), tipo, true);

            double valor = 0;

            switch (tipoEnum)
            {
                case TipoSensor.Temperatura:
                    // entre 15.0 e 30.0 
                    valor = 14 + (_random.NextDouble() * 2);
                    break;

                case TipoSensor.Humidade:
                    // entre 50% e 80% 
                    valor = 65 + (_random.NextDouble() * 10);
                    break;

                case TipoSensor.Luminosidade:
                    // vai de 0 a 1000 ou 1023
                    valor = _random.Next(200, 251);
                    break;

                default:
                    valor = 0;
                    break;
            }

            Console.WriteLine($" -> [GERADO] Tipo: {tipoEnum} | Valor Calculado: {valor}");
            var leitura = new LeituraDTO{ SensorId = sensorId, Valor = (float)Math.Round(valor, 2)};

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
