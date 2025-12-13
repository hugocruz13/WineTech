using DAL.Helpers;
using DAL.Interfaces;
using ServiceUtilizador;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class UtilizadorDAL : IUtilizadorDAL
    {
        private UtilizadorRepositoryServiceSoapClient CreateClient()
        {
            return new UtilizadorRepositoryServiceSoapClient(UtilizadorRepositoryServiceSoapClient.EndpointConfiguration.UtilizadorRepositoryServiceSoap);
        }

        public async Task<Models.Utilizador> AddUserAsync(Models.Utilizador user)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var soapModel = new ServiceUtilizador.Utilizador { Id = user.Id, Nome = user.Nome, Email = user.Email, ImgUrl = user.ImgUrl };
                var response = await client.AddUserAsync(soapModel);
                var item =  response.Body.AddUserResult;
                if (item == null) return null;
                return new Models.Utilizador { Id = item.Id, Nome = item.Nome, Email = item.Email, ImgUrl = item.ImgUrl };
            });
        }

        public async Task<Models.Utilizador> GetUserByIdAsync(string id)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var response = await client.GetUserByIdAsync(id);
                var item = response.Body.GetUserByIdResult;
                if (item == null) return null;
                return new Models.Utilizador { Id = item.Id, Nome = item.Nome, Email = item.Email, ImgUrl = item.ImgUrl };
            });
        }
    }
}