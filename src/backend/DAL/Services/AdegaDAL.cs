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
                    ImagemUrl = item.ImagemUrl,
                    Vinhos = item.Vinhos?.Select(v =>
                        new Models.Vinho
                        {
                            Id = v.Id,
                            Nome = v.Nome,
                            Produtor = v.Produtor,
                            Ano = v.Ano,
                            Tipo = v.Tipo,
                            Descricao = v.Descricao,
                            ImagemUrl = v.ImagemUrl,
                            Preco = v.Preco
                        }
                    ).ToList()
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
                    ImagemUrl = item.ImagemUrl,
                    Vinhos = item.Vinhos?.Select(v =>
                        new Models.Vinho
                        {
                            Id = v.Id,
                            Nome = v.Nome,
                            Produtor = v.Produtor,
                            Ano = v.Ano,
                            Tipo = v.Tipo,
                            Descricao = v.Descricao,
                            ImagemUrl = v.ImagemUrl,
                            Preco = v.Preco
                        }
                    ).ToList()
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
    }
}