using DAL.Interfaces;
using DAL.Helpers;
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

        public async Task<int> InserirAdega(string localizacao)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.InserirAdegaAsync(localizacao);
                return response.Body.InserirAdegaResult;
            });
        }

        public async Task<List<Models.Adega>> TodasAdegas()
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.TodasAdegasAsync();
                return response.Body.TodasAdegasResult
                .Select(a => new Models.Adega{Id = a.Id, Localizacao = a.Localizacao})
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

                return new Models.Adega { Id = item.Id, Localizacao = item.Localizacao };
            });
        }

        public async Task<bool> ModificarAdega(Models.Adega adega)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceAdega.Adega { Id = adega.Id, Localizacao = adega.Localizacao };
                var response = await client.ModificarAdegaAsync(soapModel);
                return response.Body.ModificarAdegaResult;
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