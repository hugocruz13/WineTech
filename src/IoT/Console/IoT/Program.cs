using IoT.Logica;
using IoT.Sensores;
using IoT.Workers;
using System;
using System.Globalization;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoT
{
    namespace IoT
    {
        internal class Program
        {
            static async Task Main(string[] args)
            {
                Console.WriteLine("=== IoT Gateway Iniciado ===");


                var apiClient = new ApiClient();

                var sensorWorker = new JobGet(apiClient);
                var leituraWorker = new JobPost();

                var tarefas = new[]
                {
                sensorWorker.StartAsync(),
                leituraWorker.StartAsync()
                };

                await Task.WhenAll(tarefas);
            }
        }
    }
}

