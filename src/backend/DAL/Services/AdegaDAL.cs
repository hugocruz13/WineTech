using DAL.Helpers;
using DAL.Interfaces;
using ServiceAdega;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class AdegaDAL : IAdegaDAL
    {
        private AdegaRepositoryServiceSoapClient CreateClient()
        {
            return new AdegaRepositoryServiceSoapClient(AdegaRepositoryServiceSoapClient.EndpointConfiguration.AdegaRepositoryServiceSoap);
        }

        public async Task<Models.Adega> InserirAdega(Models.Adega adega)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceAdega.Adega { Id = adega.Id, Nome = adega.Nome, Localizacao = adega.Localizacao, Capacidade = adega.Capacidade };
                var response = await client.InserirAdegaAsync(soapModel);
                var item = response.Body.InserirAdegaResult;

                if (item == null)
                    return null;

                return new Models.Adega
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Localizacao = item.Localizacao,
                    Capacidade = item.Capacidade,
                    ImagemUrl = item.ImagemUrl
                };
            });
        }

        public async Task<List<Models.Adega>> TodasAdegas()
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.TodasAdegasAsync();
                return response.Body.TodasAdegasResult
                .Select(item => new Models.Adega { Id = item.Id, Nome = item.Nome, Localizacao = item.Localizacao, Capacidade = item.Capacidade, ImagemUrl = item.ImagemUrl })
                .ToList();
            });
        }

        public async Task<Models.Adega> AdegaById(int id)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.AdegaByIdAsync(id);
                var item = response.Body.AdegaByIdResult;

                if (item == null)
                    return null;

                return new Models.Adega
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Localizacao = item.Localizacao,
                    Capacidade = item.Capacidade,
                    ImagemUrl = item.ImagemUrl
                };
            });
        }

        public async Task<Models.Adega> ModificarAdega(Models.Adega adega)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceAdega.Adega { Id = adega.Id, Nome = adega.Nome, Localizacao = adega.Localizacao, Capacidade = adega.Capacidade, ImagemUrl = adega.ImagemUrl };
                var response = await client.ModificarAdegaAsync(soapModel);
                var item = response.Body.ModificarAdegaResult;

                if (item == null)
                    return null;

                return new Models.Adega
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Localizacao = item.Localizacao,
                    Capacidade = item.Capacidade,
                    ImagemUrl = item.ImagemUrl
                };
            });
        }

        public async Task<bool> ApagarAdega(int id)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ApagarAdegaAsync(id);
                return response;
            });
        }

        public async Task<List<Models.StockResumo>> ObterResumoPorAdega(int id)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.StockAdegaAsync(id);
                return response.Body.StockAdegaResult
                .Select(item => new Models.StockResumo
                {
                    VinhoId = item.VinhoId,
                    Nome = item.Nome,
                    Produtor = item.Produtor,
                    Ano = item.Ano,
                    Tipo = item.Tipo,
                    ImagemUrl = item.ImagemUrl,
                    Preco = item.Preco,
                    Quantidade = item.Quantidade
                })
                .ToList();
            });
        }

        public async Task<bool> AdicionarStock(Models.StockInput stock)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceAdega.StockInput { AdegaId = stock.AdegaId, VinhoId = stock.VinhoId, Quantidade = stock.Quantidade };
                var response = await client.AdicionarStockAsync(soapModel);
                return response != null;
            });
        }

        public async Task<bool> AtualizarStock(Models.StockInput stock)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceAdega.StockInput { AdegaId = stock.AdegaId, VinhoId = stock.VinhoId, Quantidade = stock.Quantidade };
                var response = await client.AtualizarStockAsync(soapModel);
                return response != null;
            });
        }

        public async Task<int> ObterCapacidadeAtual(int adegaId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterOcupacaoAtualAsync(adegaId);
                return response;
            });
        }

        public async Task<List<Models.StockResumo>> ObterResumoStockTotal()
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ObterResumoStockTotalAsync();
                return response.Body.ObterResumoStockTotalResult
                .Select(item => new Models.StockResumo
                {
                    VinhoId = item.VinhoId,
                    Nome = item.Nome,
                    Produtor = item.Produtor,
                    Ano = item.Ano,
                    Tipo = item.Tipo,
                    ImagemUrl = item.ImagemUrl,
                    Preco = item.Preco,
                    Quantidade = item.Quantidade
                })
                .ToList();
            });
        }

        public async Task<bool> ApagarStock(int vinhoId)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.RemoverStockAsync(vinhoId);
                return response;
            });
        }
    }
}