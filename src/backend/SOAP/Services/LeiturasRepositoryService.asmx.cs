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

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class LeiturasRepositoryService : WebService
    {
        private readonly LeiturasRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public LeiturasRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new LeiturasRepository(connectionFactory);
        }

        [WebMethod]
        public Models.Leituras InserirLeitura(Models.Leituras leitura)
        {
            try
            {
                return _repository.InserirLeitura(leitura);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir leitura");
                throw new Exception("Erro ao inserir leitura: " + ex.Message);
            }
        }

        [WebMethod]
        public List<Models.Leituras> ObterLeiturasPorSensor(int sensorId)
        {
            try
            {
                return _repository.ObterLeiturasPorSensor(sensorId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter Sensores");
                throw new Exception("Erro ao obter Sensores: " + ex.Message);
            }
        }

        [WebMethod]
        public LeiturasStock ObterLeiturasStock(int stockId)
        {
            try
            {
                return _repository.ObterLeiturasStock(stockId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter LeiturasStock");
                throw new Exception("Erro ao obter LeiturasStock: " + ex.Message);
            }
        }
        
        [WebMethod]
        public LeiturasStock ObterLeiturasAdega(int adegaId)
        {
            try
            {
                return _repository.ObterLeiturasAdega(adegaId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao obter Leituras da Adega {adegaId}");
                throw new Exception("Erro ao obter Leituras da Adega: " + ex.Message);
            }
        }

    }
}
