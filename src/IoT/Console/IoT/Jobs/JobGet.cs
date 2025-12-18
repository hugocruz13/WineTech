using IoT.Sensores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Workers
{
    public class JobGet
    {
        private readonly ApiClient _apiClient;

        public JobGet(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("GET iniciado.");
            while (true)
            {
                try
                {
                    Console.WriteLine("A sincronizar lista de sensores...");

                    var lista = await _apiClient.GetSensores();

                    await SensorCache.VerificarNovosSensores(lista);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[GET] Erro: " + ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(45));
            }
        }
    }
}
