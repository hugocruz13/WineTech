using NLog;
using SOAP.Models;
using SOAP.Repository;
using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Threading.Tasks;

namespace SOAP.Services
{
    /// <summary>
    /// Serviço SOAP para gerir Adegas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AdegaRepositoryService : System.Web.Services.WebService
    {
        private readonly AdegaRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AdegaRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new AdegaRepository(connectionFactory);
        }

        [WebMethod]
        public async Task<int> InserirAdega(string localizacao)
        {
            if (string.IsNullOrWhiteSpace(localizacao))
                throw new ArgumentNullException(nameof(localizacao), "A localização não pode ser nula ou vazia");

            try
            {
                return await _repository.InserirAdegaAsync(localizacao);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir a adega: " + ex.Message);
            }
        }

        [WebMethod]
        public async Task<List<Adega>> TodasAdegas()
        {
            try
            {
                return await _repository.TodasAdegasAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar todas as adegas: " + ex.Message);
            }
        }

        [WebMethod]
        public async Task<Adega> AdegaById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero", nameof(id));

            try
            {
                return await _repository.AdegaByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao procurar a adega com ID {id}: " + ex.Message);
            }
        }

        [WebMethod]
        public async Task<bool> ModificarAdega(Adega adega)
        {
            if (adega == null)
                throw new ArgumentNullException(nameof(adega), "A adega não pode ser nula");

            try
            {
                var result = await _repository.ModificarAdegaAsync(adega);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao modificar a adega com ID {adega.Id}");
                return false;
            }
        }

        [WebMethod]
        public async Task<bool> ApagarAdegaAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero", nameof(id));

            try
            {
                var result = await _repository.ApagarAdegaAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Erro ao apagar a adega com ID {id}");
                return false;
            }
        }
    }
}
