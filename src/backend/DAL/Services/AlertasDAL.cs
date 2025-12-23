using DAL.Helpers;
using DAL.Interfaces;
using ServiceAlertas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace DAL.Services
{
    public class AlertasDAL : IAlertasDAL
    {
        private AlertasRepositoryServiceSoapClient CreateClient()
        {
            return new AlertasRepositoryServiceSoapClient(AlertasRepositoryServiceSoapClient.EndpointConfiguration.AlertasRepositoryServiceSoap);
        }

        public async Task<Models.Alertas> InserirAlerta(Models.Alertas alerta)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceAlertas.Alertas
                {
                    SensoresId = alerta.SensoresId,
                    TipoAlerta = alerta.TipoAlerta,
                    Mensagem = alerta.Mensagem
                };

                var response = await client.InserirAlertaAsync(soapModel);
                var item = response.Body.InserirAlertaResult;

                if (item == null)
                    return null;

                return new Models.Alertas{ Id = item.Id,SensoresId = item.SensoresId,TipoAlerta = item.TipoAlerta,Mensagem = item.Mensagem,DataHora = item.DataHora, Resolvido = item.Resolvido};
            });
        }
        public async Task<List<Models.Alertas>> ObterAlertasPorSensor(int sensorId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterAlertasPorSensorAsync(sensorId);

                var items = response.Body.ObterAlertasPorSensorResult;

                if (items == null)
                    return new List<Models.Alertas>();

                return items.Select(item => new Models.Alertas
                {
                    Id = item.Id,
                    SensoresId = item.SensoresId,
                    TipoAlerta = item.TipoAlerta,
                    Mensagem = item.Mensagem,
                    DataHora = item.DataHora,
                    Resolvido = item.Resolvido
                }).ToList();
            });
        }

        public async Task<List<Models.AlertaComSensor>> GetAllAlertas()
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.GetAllAlertasAsync();
                var items = response.Body.GetAllAlertasResult;
                if (items == null)
                    return new List<Models.AlertaComSensor>();
                return items.Select(item => new Models.AlertaComSensor
                {
                    Id = item.Id,
                    TipoAlerta = item.TipoAlerta,
                    Mensagem = item.Mensagem,
                    DataHora = item.DataHora,
                    Resolvido = item.Resolvido,
                    SensoresId = item.SensoresId,
                    IdentificadorHardware = item.IdentificadorHardware,
                    TipoSensor = item.TipoSensor,
                    AdegaId = item.AdegaId
                }).ToList();
            });
        }

        public async Task<bool> ResolverAlerta(int alertaId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ResolverAlertaAsync(alertaId);
                return response;
            });
        }
    }
}
