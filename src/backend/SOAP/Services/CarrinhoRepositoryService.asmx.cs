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

    public class CarrinhoRepositoryService : WebService
    {
        private readonly CarrinhoRepository _repository;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public CarrinhoRepositoryService()
        {
            var connectionFactory = new DbConnectionFactory();
            _repository = new CarrinhoRepository(connectionFactory);
        }

        [WebMethod]
        public List<Carrinho> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            try
            {
                return _repository.ObterCarrinhoPorUtilizador(utilizadoresId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter carrinho");
                throw new Exception("Erro ao obter carrinho: " + ex.Message);
            }
        }
        [WebMethod]
        public List<Carrinho> InserirItem(Carrinho itemCarrinho)
        {
            try
            {
                return _repository.InserirItem(itemCarrinho);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir vinho no carrinho");
                throw new Exception("Erro ao inserir vinho no carrinho: " + ex.Message);
            }
        }
        [WebMethod]
        public List<Carrinho> AtualizarItem(Carrinho itemCarrinho)
        {
            try
            {
                return _repository.AtualizarItem(itemCarrinho);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao atualizar a quantidade de vinho no carrinho");
                throw new Exception("Erro ao atualizar a quantidade de vinho no carrinho: " + ex.Message);
            }
        }
        [WebMethod]
        public bool EliminarItem(int vinhoId, string utilizadoresId)
        {
            try
            {
                return _repository.EliminarItem(vinhoId, utilizadoresId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao apagar vinho do carrinho");
                throw new Exception("Erro ao apagar vinho do carrinho: " + ex.Message);
            }
        }

    }
}
