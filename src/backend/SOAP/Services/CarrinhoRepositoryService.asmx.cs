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
    /// Descrição resumida de CarrinhoRepositoryService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
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
    }
}
