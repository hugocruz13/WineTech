using IoT.Logica;
using IoT.Sensores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT.Workers
{
    public class JobPost
    {
        public async Task StartAsync()
        {
            Console.WriteLine("POST iniciado.");
            while (true)
            {
                try
                {
                    if (SensorCache.SensoresEmMemoria.Count > 0)
                    {
                        Console.WriteLine("A processar leituras...");

                        foreach (var sensor in SensorCache.SensoresEmMemoria)
                        {
                            await ValueResolver.ConfigurarSensor(sensor);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[POST] Erro: " + ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}
