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
        public List<Carrinho> ObterCarrinho(int utilizadoresId)
        {
            try
            {
                return _repository.ObterCarrinho(utilizadoresId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao obter carrinho");
                throw new Exception("Erro ao obter carrinho: " + ex.Message);
            }
        }
        [WebMethod]
        public List<Carrinho> InserirItem(int utilizadoresId, int vinhoId, int quantidade)
        {
            try
            {
                return _repository.InserirItem(utilizadoresId, vinhoId, quantidade);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao inserir vinho no carrinho");
                throw new Exception("Erro ao inserir vinho no carrinho: " + ex.Message);
            }
        }
        [WebMethod]
        public List<Carrinho> AtualizarItem(int itemId, int utilizadoresId, int quantidade)
        {
            try
            {
                return _repository.AtualizarItem(itemId, utilizadoresId, quantidade);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao atualizar a quantidade de vinho no carrinho");
                throw new Exception("Erro ao atualizar a quantidade de vinho no carrinho: " + ex.Message);
            }
        }
        [WebMethod]
        public bool EliminarItem(int itemId, int utilizadoresId)
        {
            try
            {
                return _repository.EliminarItem(itemId, utilizadoresId);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Erro ao apagar vinho do carrinho");
                throw new Exception("Erro ao apagar vinho do carrinho: " + ex.Message);
            }
        }

    }
}
