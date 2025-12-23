using IoT.Api.ApiModels;
using IoT.Simulation;
using System;

using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoT.Hardware
{
    public class ArduinoReader
    {
        private static SerialPort porta;
        private static bool ativo = false;

        private static int idTemperatura = 0, idHumidade = 0, idLuz = 0, adegaId = 0;
        public static void ConfigurarSensorReal(int sensorId, string tipo, int adegaId)
        {
            TipoSensor tipoEnum = (TipoSensor)Enum.Parse(typeof(TipoSensor), tipo, true);
            switch (tipoEnum)
            {
                case TipoSensor.Temperatura:
                    idTemperatura = sensorId;
                    break;
                case TipoSensor.Humidade:
                    idHumidade = sensorId;
                    break;
                case TipoSensor.Luminosidade:
                    idLuz= sensorId;
                    break;
                default:
                    Console.WriteLine($" -> Tipo desconhecido: '{tipoEnum}'");
                    break;
            }
            ArduinoReader.adegaId = adegaId;
            if (!ativo)
            {
                LigarArduino();
            }
        }
        private static void LigarArduino()
        {
            try
            {
                porta = new SerialPort("COM3", 9600);
                porta.Open();
                ativo = true;

                Console.WriteLine(" -> [ARDUINO] Porta COM3 ligada. A ler dados...");

                Task.Run(async () => await LoopDeLeitura());
            }
            catch (Exception ex)
            {
                Console.WriteLine($" -> [ERRO ARDUINO] Falha ao abrir COM3: {ex.Message}");
            }
        }
        private static async Task LoopDeLeitura()
        {
            while (ativo && porta.IsOpen)
            {
                try
                {
                    string linha = porta.ReadLine();

                    string[] valores = linha.Split(',');

                    float temp = float.Parse(valores[0], CultureInfo.InvariantCulture);
                    float hum = float.Parse(valores[1], CultureInfo.InvariantCulture);
                    float ldr = float.Parse(valores[2], CultureInfo.InvariantCulture);


                    if (idTemperatura > 0)
                        await EnviarLeitura(idTemperatura, temp, TipoSensor.Temperatura, adegaId);

                    if (idHumidade > 0)
                        await EnviarLeitura(idHumidade, hum, TipoSensor.Humidade, adegaId);
                    
                    if (idLuz > 0)
                        await EnviarLeitura(idLuz, ldr, TipoSensor.Luminosidade, adegaId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" -> [ERRO LEITURA] {ex.Message}");
                }
            }
        }
        private static async Task EnviarLeitura(int id, float valor, TipoSensor tipo, int adegaId)
        {
            var leitura = new LeituraDTO
            {
                SensorId = id,
                Valor = (float)Math.Round(valor, 2),
                Tipo = tipo.ToString(),
                AdegaId = adegaId
            };

            try
            {
                Console.WriteLine($" -> [ARDUINO ENVIAR] Sensor {id} ({leitura.Tipo}): {valor}");
                await ApiClient.InserirLeitura(leitura);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro envio API: {ex.Message}");
            }
        }
    }
}
