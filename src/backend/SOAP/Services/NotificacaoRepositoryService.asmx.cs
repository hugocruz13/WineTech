using NLog;
using SOAP.Models;
using SOAP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SOAP.Services
{
    /// <summary>
    /// Summary description for NotificacaoRepositoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotificacaoRepositoryService : System.Web.Services.WebService
    {

        private readonly NotificacaoRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public NotificacaoRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new NotificacaoRepository(connectionFactory);
        }

        [WebMethod]
        public Notificacao InserirNotificacao(Notificacao notificacao)
        {
            if (notificacao == null)
            {
                throw new ArgumentNullException(nameof(notificacao));
            }

            try
            {
                return _repository.InserirNotificacao(notificacao);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir notificação.");
                throw;
            }
        }

        [WebMethod]
        public List<Notificacao> ObterNotificacoesPorUtilizador(string utilizadorId)
        {
            if (string.IsNullOrWhiteSpace(utilizadorId))
            {
                throw new ArgumentException("O ID do utilizador não pode ser nulo ou vazio.", nameof(utilizadorId));
            }
            try
            {
                return _repository.ObterNotificacoesPorUtilizador(utilizadorId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter notificações para o utilizador.");
                throw;
            }
        }

        [WebMethod]
        public Notificacao MarcarNotificacaoComoLida(int notificacaoId)
        {
            if (notificacaoId <= 0)
            {
                throw new ArgumentException("O ID da notificação deve ser maior que zero.", nameof(notificacaoId));
            }

            try
            {
                return _repository.MarcarComoLida(notificacaoId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao marcar notificação como lida.");
                throw;
            }
        }
    }
}
