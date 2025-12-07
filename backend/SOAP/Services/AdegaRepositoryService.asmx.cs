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
    public class AdegaRepositoryService : WebService
    {
        private readonly AdegaRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AdegaRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new AdegaRepository(connectionFactory);
        }

        [WebMethod]
        public int InserirAdega(string localizacao)
        {
            if (string.IsNullOrWhiteSpace(localizacao))
                throw new ArgumentException("A localização não pode ser nula ou vazia");

            try
            {
                return _repository.InserirAdega(localizacao);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir a adega");
                throw new Exception("Erro ao inserir a adega: " + ex.Message);
            }
        }

        [WebMethod]
        public List<Adega> TodasAdegas()
        {
            try
            {
                return _repository.TodasAdegas();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao listar todas as adegas");
                throw new Exception("Erro ao listar todas as adegas: " + ex.Message);
            }
        }

        [WebMethod]
        public Adega AdegaById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero");

            try
            {
                return _repository.AdegaById(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao procurar a adega com ID {id}");
                throw new Exception($"Erro ao procurar a adega com ID {id}: " + ex.Message);
            }
        }

        [WebMethod]
        public bool ModificarAdega(Adega adega)
        {
            if (adega == null)
                throw new ArgumentNullException(nameof(adega), "A adega não pode ser nula");

            try
            {
                return _repository.ModificarAdega(adega);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao modificar a adega com ID {adega.Id}");
                throw new Exception($"Erro ao modificar a adega com ID {adega.Id}: {ex.Message}");
            }
        }

        [WebMethod]
        public bool ApagarAdega(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero");

            try
            {
                return _repository.ApagarAdega(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao apagar a adega com ID {id}");
                throw new Exception($"Erro ao apagar a adega com ID {id}: {ex.Message}");
            }
        }
    }
}
