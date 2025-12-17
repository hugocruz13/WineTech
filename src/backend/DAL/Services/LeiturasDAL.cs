using DAL.Helpers;
using DAL.Interfaces;
using ServiceLeituras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class LeiturasDAL : ILeiturasDAL
    {
        private LeiturasRepositoryServiceSoapClient CreateClient()
        {
            return new LeiturasRepositoryServiceSoapClient(LeiturasRepositoryServiceSoapClient.EndpointConfiguration.LeiturasRepositoryServiceSoap);
        }

        public async Task<Models.Leituras> InserirLeitura(Models.Leituras leitura)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceLeituras.Leituras{ SensorId = leitura.SensorId, Valor = leitura.Valor };
                var response = await client.InserirLeituraAsync(soapModel);
                var item = response.Body.InserirLeituraResult;

                if (item == null)
                    return null;

                return new Models.Leituras{ Id = item.Id, SensorId = item.SensorId, Valor = (float)item.Valor, DataHora = item.DataHora };
            });
        }

        public async Task<List<Models.Leituras>> ObterLeiturasPorSensor(int sensorId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterLeiturasPorSensorAsync(sensorId);
                var items = response.Body.ObterLeiturasPorSensorResult;

                if (items == null)
                    return new List<Models.Leituras>();

                return items.Select(item => new Models.Leituras{ Id = item.Id,SensorId = item.SensorId,Valor = (float)item.Valor,DataHora = item.DataHora}).ToList();
            });
        }
    }
}
