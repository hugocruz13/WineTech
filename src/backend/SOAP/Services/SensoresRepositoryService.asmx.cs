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
    public class SensoresRepositoryService : WebService
    {
        private readonly SensoresRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SensoresRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new SensoresRepository(connectionFactory);
        }

        [WebMethod]
        public Models.Sensores InserirSensor(Models.Sensores sensor)
        {
            if (sensor == null)
                throw new ArgumentException("O sensor não pode ser nulo.");

            try
            {
                return _repository.InserirSensor(sensor);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir o sensor.");
                throw new Exception("Erro ao inserir o sensor: " + ex.Message);
            }
        }

        [WebMethod]
        public List<Models.Sensores> TodosSensores()
        {
            try
            {
                return _repository.TodosSensores();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao listar todos os sensores");
                throw new Exception("Erro ao listar todos os sensores: " + ex.Message);
            }
        }
    }
}
