using DAL.Helpers;
using DAL.Interfaces;
using ServiceCompra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class CompraDAL : ICompraDAL
    {
        private CompraRepositoryServiceSoapClient CreateClient()
        {
            return new CompraRepositoryServiceSoapClient(CompraRepositoryServiceSoapClient.EndpointConfiguration.CompraRepositoryServiceSoap);
        }

        public async Task<Models.Compra> CriarCompra(Models.Compra compra)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceCompra.Compra { UtilizadorId = compra.UtilizadorId, Estado = compra.Estado };
                var response = await client.CriarCompraAsync(soapModel);
                var item = response.Body.CriarCompraResult;

                if (item == null)
                    return null;

                return new Models.Compra
                {
                    Id = item.Id,
                    UtilizadorId = item.UtilizadorId,
                    ValorTotal = item.ValorTotal,
                    DataCompra = item.DataCompra,
                    Estado = item.Estado,
                    cartao = item.cartao
                };
            });
        }

        public async Task<List<int>> ObterStockIds(Models.StockInput stockInput)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapInput = new ServiceCompra.StockInput { VinhoId = stockInput.VinhoId, Quantidade = stockInput.Quantidade };
                var response = await client.ObterStockIdsAsync(soapInput);
                return response.Body.ObterStockIdsResult;
            });
        }

        public async Task<bool> CriarLinha(Models.LinhaCompra linha)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapStockIds = new ServiceCompra.ArrayOfInt();
                if (linha.stockIds != null)
                {
                    foreach (var id in linha.stockIds)
                    {
                        soapStockIds.Add(id);
                    }
                }
                var soapModel = new ServiceCompra.LinhaCompra { CompraId = linha.CompraId, stockIds = soapStockIds };
                var response = await client.AdicionarItensCompraAsync(soapModel);
                return response.Body.AdicionarItensCompraResult;
            });
        }

        public async Task<bool> FinalizarCompra(Models.LinhaCompra linha)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapStockIds = new ServiceCompra.ArrayOfInt();
                if (linha.stockIds != null)
                {
                    foreach (var id in linha.stockIds)
                    {
                        soapStockIds.Add(id);
                    }
                }
                var soapModel = new ServiceCompra.LinhaCompra { CompraId = linha.CompraId, stockIds = soapStockIds };
                var response = await client.FinalizarCompraAsync(soapModel);
                return response.Body.FinalizarCompraResult;
            });
        }

        public async Task<bool> AtualizarValorTotal(Models.Compra compra)
        {
            return await SoapClientHelper.ExecuteAsync<CompraRepositoryServiceSoapClient, bool>(CreateClient, async client =>
            {
                var soapModel = new ServiceCompra.Compra { Id = compra.Id, UtilizadorId = compra.UtilizadorId, Estado = compra.Estado,ValorTotal = compra.ValorTotal, DataCompra = compra.DataCompra , cartao = compra.cartao};
                var response = await client.AtualizarValorTotalAsync(soapModel);
                return response.Body.AtualizarValorTotalResult;
            });
        }

        public async Task<List<Models.Compra>> ObterComprasUtilizador(string utilizadorId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterComprasPorUtilizadorAsync(utilizadorId);
                var soapList = response.Body.ObterComprasPorUtilizadorResult;
                var result = new List<Models.Compra>();

                foreach (var item in soapList)
                {
                    result.Add(new Models.Compra
                    {
                        Id = item.Id,
                        ValorTotal = item.ValorTotal,
                        UtilizadorId = item.UtilizadorId,
                        DataCompra = item.DataCompra
                    });
                }
                return result;
            });
        }

        public async Task<List<Models.CompraDetalhe>> ObterDetalhesCompra(int compraId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterLinhasPorCompraAsync(compraId);
                var soapList = response.Body.ObterLinhasPorCompraResult;
                var result = new List<Models.CompraDetalhe>();
                foreach (var item in soapList)
                {
                    result.Add(new Models.CompraDetalhe
                    {
                        IdCompra = item.IdCompra,
                        ValorTotal = item.ValorTotal,
                        DataCompra = item.DataCompra,
                        IdVinho = item.IdVinho,
                        Nome = item.Nome,
                        Produtor = item.Produtor,
                        Ano = item.Ano,
                        Tipo = item.Tipo,
                        Quantidade = item.Quantidade,
                        Preco = item.Preco,
                        ImgVinho = item.ImgVinho,
                        NomeUtilizador = item.NomeUtilizador,
                        EmailUtilizador = item.EmailUtilizador,
                        ImagemUtilizador = item.ImagemUtilizador,
                        StockId = item.StockId,
                        Cartao = item.Cartao,
                        idUtilizador = item.idUtilizador
                    });
                }
                return result;
            });
        }
    }
}
