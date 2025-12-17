using ServiceSensores;
using DAL.Helpers;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class SensoresDAL : ISensoresDAL
    {
        private SensoresRepositoryServiceSoapClient CreateClient()
        {
            return new SensoresRepositoryServiceSoapClient(SensoresRepositoryServiceSoapClient.EndpointConfiguration.SensoresRepositoryServiceSoap);
        }
        public async Task<Models.Sensores> InserirSensor(Models.Sensores sensor)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceSensores.Sensores {IdentificadorHardware = sensor.IdentificadorHardware, Tipo = sensor.Tipo, Estado = sensor.Estado, ImagemUrl = sensor.ImagemUrl, AdegaId = sensor.AdegaId };
                var response = await client.InserirSensorAsync(soapModel);
                var item = response.Body.InserirSensorResult;

                if (item == null)
                    return null;

                return new Models.Sensores { Id = item.Id, IdentificadorHardware = item.IdentificadorHardware, Tipo = item.Tipo, Estado = item.Estado, ImagemUrl = item.ImagemUrl, AdegaId = item.AdegaId };
            });
        }
        public async Task<List<Models.Sensores>> TodosSensores()
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.TodosSensoresAsync();
                return response.Body.TodosSensoresResult
                .Select(item => new Models.Sensores { Id = item.Id, IdentificadorHardware = item.IdentificadorHardware, Tipo = item.Tipo, Estado = item.Estado, ImagemUrl = item.ImagemUrl, AdegaId = item.AdegaId })
                .ToList();
            });
        }
        public async Task<List<Models.Sensores>> ObterSensoresPorAdega(int adegaId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterSensoresPorAdegaAsync(adegaId);

                return response.Body.ObterSensoresPorAdegaResult
                .Select(item => new Models.Sensores { Id = item.Id, IdentificadorHardware = item.IdentificadorHardware, Tipo = item.Tipo, Estado = item.Estado, ImagemUrl = item.ImagemUrl, AdegaId = item.AdegaId })
                .ToList();
            });
        }
        public async Task<List<Models.Leituras>> ObterLeiturasPorSensor(int sensorId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterLeiturasPorSensorAsync(sensorId);

                return response.Body.ObterLeiturasPorSensorResult
                    .Select(item => new Models.Leituras
                    {
                        Id = item.Id,
                        SensorId = item.SensorId,
                        Valor = (float)item.Valor,
                        DataHora = item.DataHora
                    })
                    .ToList();
            });
        }
    }
}
