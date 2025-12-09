using NLog;
using SOAP.Models;
using SOAP.Repository;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace SOAP.Services
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class VinhoRepositoryService : WebService
    {
        private readonly VinhoRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public VinhoRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new VinhoRepository(connectionFactory);
        }
        [WebMethod]
        public Vinho InserirVinho(Vinho vinho)
        {
            if (vinho == null)
                throw new ArgumentException("O vinho não pode ser nulo.");

            try
            {
                return _repository.InserirVinho(vinho);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir o vinho");
                throw new Exception("Erro ao inserir o vinho: " + ex.Message);
            }
        }
        [WebMethod]
        public Vinho ModificarVinho(Vinho vinho)
        {
            if (vinho == null)
                throw new ArgumentNullException(nameof(vinho), "O vinho não pode ser nulo");

            try
            {
                return _repository.ModificarVinho(vinho);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao modificar a vinho com ID {vinho.Id}");
                throw new Exception($"Erro ao modificar a vinho com ID {vinho.Id}: {ex.Message}");
            }
        }
        [WebMethod]
        public Vinho VinhoById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero");

            try
            {
                return _repository.VinhoById(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao procurar a adega com ID {id}");
                throw new Exception($"Erro ao procurar a adega com ID {id}: " + ex.Message);
            }
        }
        [WebMethod]
        public List<Vinho> TodosVinhos()
        {
            try
            {
                return _repository.TodosVinhos();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao listar todas as adegas");
                throw new Exception("Erro ao listar todas as adegas: " + ex.Message);
            }
        }
        [WebMethod]
        public bool ApagarVinho(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero");

            try
            {
                return _repository.ApagarVinho(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao apagar o vinho com ID {id}");
                throw new Exception($"Erro ao apagar o vinho com ID {id}: {ex.Message}");
            }
        }
    }

}
