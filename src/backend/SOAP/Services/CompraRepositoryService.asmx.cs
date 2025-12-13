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
    /// Summary description for CompraRepositoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CompraRepositoryService : System.Web.Services.WebService
    {
        private readonly CompraRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CompraRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new CompraRepository(connectionFactory);
        }

        [WebMethod]
        public List<int> ObterStockIds(StockInput stockInput)
        {
            if (stockInput == null)
                throw new ArgumentNullException(nameof(stockInput), "O input de stock não pode ser nulo");

            try
            {
                return _repository.ObterStockIds(stockInput);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao encontar id's");
                throw new Exception("\"Erro ao encontar id': " + ex.Message);
            }
        }

        [WebMethod]
        public Compra CriarCompra(Compra compra)
        {
            if (compra == null)
                throw new ArgumentNullException(nameof(compra), "A compra não pode ser nula");

            if (string.IsNullOrEmpty(compra.UtilizadorId))
                throw new ArgumentException("O ID do utilizador deve ser maior que zero", nameof(compra.UtilizadorId));

            try
            {
                return _repository.CriarCompra(compra);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao criar a compra");
                throw new Exception("Erro ao criar a compra: " + ex.Message);
            }
        }

        [WebMethod]
        public bool AdicionarItensCompra(LinhaCompra linha)
        {
            if (linha.CompraId <= 0)
                throw new ArgumentException("O ID da compra deve ser maior que zero", nameof(linha.CompraId));
            if (linha.stockIds == null || !linha.stockIds.Any())
                throw new ArgumentException("A lista de IDs de stock não pode ser nula ou vazia", nameof(linha.stockIds));

            try
            {
                return _repository.CriarLinhas(linha);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao adicionar itens à compra");
                throw new Exception("Erro ao adicionar itens à compra: " + ex.Message);
            }
        }

        [WebMethod]
        public bool FinalizarCompra(LinhaCompra linha)
        {
            if (linha.CompraId <= 0)
                throw new ArgumentException("O ID da compra deve ser maior que zero", nameof(linha.CompraId));
            if (linha.stockIds == null || !linha.stockIds.Any())
                throw new ArgumentException("A lista de IDs de stock não pode ser nula ou vazia", nameof(linha.stockIds));

            try
            {
                return _repository.Finalizar(linha);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao finalizar a compra");
                throw new Exception("Erro ao finalizar a compra: " + ex.Message);
            }
        }

        [WebMethod]
        public bool AtualizarValorTotal(Compra compra)
        {
            if (compra == null)
                throw new ArgumentNullException("A compra não pode ser nula");

            if (compra.Id <= 0)
                throw new ArgumentException("O ID da compra deve ser maior que zero", nameof(compra.Id));

            if (compra.ValorTotal <= 0)
                throw new ArgumentException("O valor total da compra deve ser maior que zero", nameof(compra.ValorTotal));

            try
            {
                return _repository.AtualizarValorTotal(compra);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao cancelar a compra");
                throw new Exception("Erro ao cancelar a compra: " + ex.Message);
            }
        }

        [WebMethod]
        public List<Compra> ObterComprasPorUtilizador(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("O ID do utilizador não pode ser nulo ou vazio", nameof(userId));
            try
            {
                return _repository.ObterComprasPorUtilizador(userId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter a compra");
                throw new Exception("Erro ao obter a compra: " + ex.Message);
            }
        }

        [WebMethod]
        public List<CompraDetalhe> ObterLinhasPorCompra(int compraId)
        {
            if (compraId <= 0)
                throw new ArgumentException("O ID da compra deve ser maior que zero", nameof(compraId));
            try
            {
                return _repository.CompraDetalhes(compraId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter as linhas da compra");
                throw new Exception("Erro ao obter as linhas da compra: " + ex.Message);
            }
        }
    }
}
