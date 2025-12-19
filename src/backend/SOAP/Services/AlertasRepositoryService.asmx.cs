using NLog;
using SOAP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SOAP.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AlertasRepositoryService : WebService
    {
        private readonly AlertasRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AlertasRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new AlertasRepository(connectionFactory);
        }
        [WebMethod]
        public Models.Alertas InserirAlerta(Models.Alertas alerta)
        {
            try
            {
                return _repository.InserirAlerta(alerta);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir alerta");
                throw new Exception("Erro ao inserir alerta: " + ex.Message);
            }
        }
    }
}
