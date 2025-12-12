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
            SerialPort porta = new SerialPort("COM3", 9600);
            porta.Open();

            HttpClient client = new HttpClient();

            while (true)
            {
                try
                {
                    string linha = porta.ReadLine();
                    Console.WriteLine(linha);

                    string[] valores = linha.Split(',');

                    float temp = float.Parse(valores[0], CultureInfo.InvariantCulture);
                    float humi = float.Parse(valores[1], CultureInfo.InvariantCulture);
                    int ldr = int.Parse(valores[2]);

                    var dados = new
                    {
                        temperature = temp,
                        humidity = humi,
                        lightIntensity = ldr
                    };

                    string json = JsonSerializer.Serialize(dados);

                    string apiUrl = "https://localhost:7148/api/sensor/data";

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    Console.WriteLine(response.StatusCode);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro: " + ex.Message);
                }
            }
        }
    }
}
