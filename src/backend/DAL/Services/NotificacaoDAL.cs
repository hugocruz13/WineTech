using DAL.Helpers;
using DAL.Interfaces;
using ServiceNotificacao;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class NotificacaoDAL : INotificacaoDAL
    {
        private NotificacaoRepositoryServiceSoapClient CreateClient()
        {
            return new NotificacaoRepositoryServiceSoapClient(NotificacaoRepositoryServiceSoapClient.EndpointConfiguration.NotificacaoRepositoryServiceSoap);
        }

        public async Task<Models.Notificacao> InserirNotificacao(Models.Notificacao notificacao)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceNotificacao.Notificacao
                {
                    Id = notificacao.Id,
                    Titulo = notificacao.Titulo,
                    Mensagem = notificacao.Mensagem,
                    Tipo = (ServiceNotificacao.TipoNotificacao)notificacao.Tipo, 
                    Lida = notificacao.Lida,
                    CreatedAt = notificacao.CreatedAt,
                    UtilizadorId = notificacao.UtilizadorId
                };
                var response = await client.InserirNotificacaoAsync(soapModel);
                var item = response.Body.InserirNotificacaoResult;
                if (item == null)
                    return null;
                return new Models.Notificacao
                {
                    Id = item.Id,
                    Titulo = item.Titulo,
                    Mensagem = item.Mensagem,
                    Tipo = (Models.TipoNotificacao)item.Tipo,
                    Lida = item.Lida,
                    CreatedAt = item.CreatedAt,
                    UtilizadorId = item.UtilizadorId
                };
            });
        }

        public async Task<List<Models.Notificacao>> ObterNotificacoesPorUtilizador(string utilizadorId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterNotificacoesPorUtilizadorAsync(utilizadorId);
                return response.Body.ObterNotificacoesPorUtilizadorResult
                .Select(item => new Models.Notificacao { Id = item.Id, Titulo = item.Titulo, Mensagem = item.Mensagem, Tipo = (Models.TipoNotificacao)item.Tipo, Lida = item.Lida, CreatedAt = item.CreatedAt, UtilizadorId = item.UtilizadorId })
                .ToList();

            });
        }

        public async Task<bool> MarcarNotificacaoComoLida(int idNotificacao)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.MarcarNotificacaoComoLidaAsync(idNotificacao);
                return response;
            });
        }
    }
}
