using IoT.Api.ApiModels;
using IoT.Sensores;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ApiClient
{
    private static readonly JsonSerializerOptions _jsonOptions =
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public async Task InserirLeitura(IoT.Api.ApiModels.LeituraDTO leitura)
    {
        var client = new HttpClient();

        string json = JsonSerializer.Serialize(leitura, _jsonOptions);
        Console.WriteLine("Simular envio: " + json);

        string apiUrl = "https://localhost:7148/api/leituras";

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(apiUrl, content);

        Console.WriteLine("Status: " + response.StatusCode);
    }

    public async Task<List<SensorDTO>> GetSensores()
    {
        var client = new HttpClient();
        string apiUrl = "https://localhost:7148/api/sensores";

        var response = await client.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var sensores = JsonSerializer.Deserialize<List<SensorDTO>>(json, _jsonOptions) ?? new List<SensorDTO>();
        SensorCache.VerificarNovosSensores(sensores);

        return sensores;
    }
}
