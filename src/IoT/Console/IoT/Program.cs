using IoT.Logica;
using IoT.Sensores;
using System;
using System.Globalization;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoT
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== IoT iniciado ===");
            var apiClient = new ApiClient(); 

            // Ciclo infinito
            while (true)
            {
                try
                {
                    Console.WriteLine($"\n--- Ciclo iniciado: {DateTime.Now.ToLongTimeString()} ---");


                    var listaSensores = await apiClient.GetSensores();


                    await SensorCache.VerificarNovosSensores(listaSensores);


                    if (SensorCache.SensoresEmMemoria.Count > 0)
                    {
                        foreach (var sensor in SensorCache.SensoresEmMemoria)
                        {

                            await ValueResolver.ConfigurarSensor(sensor);
                        }
                    }
                    else
                    {
                        Console.WriteLine("-> Nenhum sensor em memória. À espera de configuração...");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro no ciclo: " + ex.Message);
                }

                Console.WriteLine("A aguardar 5 segundos...");
                await Task.Delay(5000);
            }
        }
    }
}

