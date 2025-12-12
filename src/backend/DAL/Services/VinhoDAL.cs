using DAL.Helpers;
using DAL.Interfaces;
using ServiceAdega;
using ServiceVinho;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class VinhoDAL : IVinhoDAL
    {
        private VinhoRepositoryServiceSoapClient CreateClient()
        {
            return new VinhoRepositoryServiceSoapClient(VinhoRepositoryServiceSoapClient.EndpointConfiguration.VinhoRepositoryServiceSoap);
        }
        public async Task<Models.Vinho> InserirVinho(Models.Vinho vinho)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceVinho.Vinho { Id = vinho.Id, Nome = vinho.Nome, Produtor = vinho.Produtor, Ano = vinho.Ano, Tipo = vinho.Tipo, Descricao = vinho.Descricao, ImagemUrl = vinho.ImagemUrl, Preco = vinho.Preco };
                var response = await client.InserirVinhoAsync(soapModel);
                var item = response.Body.InserirVinhoResult;

                if (item == null)
                    return null;

                return new Models.Vinho { Id = item.Id, Nome = item.Nome, Produtor = item.Produtor, Ano = item.Ano, Tipo = item.Tipo, Descricao = item.Descricao, ImagemUrl = item.ImagemUrl, Preco = item.Preco };
            });
        }
        public async Task<Models.Vinho> ModificarVinho(Models.Vinho vinho)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceVinho.Vinho { Id = vinho.Id, Nome = vinho.Nome, Produtor = vinho.Produtor, Ano = vinho.Ano, Tipo = vinho.Tipo, Descricao = vinho.Descricao, ImagemUrl = vinho.ImagemUrl, Preco = vinho.Preco };
                var response = await client.ModificarVinhoAsync(soapModel);
                var item = response.Body.ModificarVinhoResult;

                if (item == null)
                    return null;

                return new Models.Vinho { Id = item.Id, Nome = item.Nome, Produtor = item.Produtor, Ano = item.Ano, Tipo = item.Tipo, Descricao = item.Descricao, ImagemUrl = item.ImagemUrl, Preco = item.Preco};
            });
        }
        public async Task<Models.Vinho> VinhoById(int id)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.VinhoByIdAsync(id);
                var item = response.Body.VinhoByIdResult;

                if (item == null)
                    return null;

                return new Models.Vinho { Id = item.Id, Nome = item.Nome, Produtor = item.Produtor, Ano = item.Ano, Tipo = item.Tipo, Descricao = item.Descricao, Preco = item.Preco };
            });
        }
        public async Task<List<Models.Vinho>> TodosVinhos()
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.TodosVinhosAsync();
                return response.Body.TodosVinhosResult
                .Select(item => new Models.Vinho { Id = item.Id, Nome = item.Nome, Produtor = item.Produtor, Ano = item.Ano, Tipo = item.Tipo, Descricao = item.Descricao, Preco = item.Preco })
                .ToList();
            });
        }
        public async Task<bool> ApagarVinho(int id)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.ApagarVinhoAsync(id);
                return response;
            });
        }
    }
}
