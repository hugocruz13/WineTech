using ServiceCarrinho;
using DAL.Helpers;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Services
{
    public class CarrinhoDAL : ICarrinhoDAL
    {
        private CarrinhoRepositoryServiceSoapClient CreateClient()
        {
            return new CarrinhoRepositoryServiceSoapClient(CarrinhoRepositoryServiceSoapClient.EndpointConfiguration.CarrinhoRepositoryServiceSoap);
        }
        public async Task<List<Models.Carrinho>> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterCarrinhoPorUtilizadorAsync(utilizadoresId);

                return response.Body.ObterCarrinhoPorUtilizadorResult
                .Select(item => new Models.Carrinho { Id = item.Id, VinhosId = item.VinhosId, UtilizadoresId = item.UtilizadoresId, Quantidade = item.Quantidade })
                .ToList();
            });
        }
        public async Task<List<Models.Carrinho>> InserirItem(Models.Carrinho itemCarrinho)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceCarrinho.Carrinho{Id = itemCarrinho.Id,VinhosId = itemCarrinho.VinhosId,UtilizadoresId = itemCarrinho.UtilizadoresId,Quantidade = itemCarrinho.Quantidade};

                var response = await client.InserirItemAsync(soapModel);

                var items = response.Body.InserirItemResult;
                if (items == null)
                    return null;

                return items
                    .Select(x => new Models.Carrinho
                    {
                        Id = x.Id,
                        VinhosId = x.VinhosId,
                        UtilizadoresId = x.UtilizadoresId,
                        Quantidade = x.Quantidade
                    })
                    .ToList();
            });
        }
        public async Task<List<Models.Carrinho>> AtualizarItem(Models.Carrinho itemCarrinho)
        {
            await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceCarrinho.Carrinho { Id = itemCarrinho.Id, VinhosId = itemCarrinho.VinhosId, UtilizadoresId = itemCarrinho.UtilizadoresId, Quantidade = itemCarrinho.Quantidade };
                var response = await client.AtualizarItemAsync(soapModel);

                return true;
            });

            return await ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId);
        }
    }
}
