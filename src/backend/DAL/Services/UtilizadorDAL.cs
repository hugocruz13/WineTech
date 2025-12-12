using DAL.Interfaces;
using DAL.Helpers; 
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

        public async Task<int> AddUserAsync(Models.Utilizador user)
        {
            return await SoapClientHelper.ExecuteAsync(CreateClient, async client =>
            {
                var userDto = new ServiceUtilizador.Utilizador
                {
                    Auth0UserId = user.Auth0UserId,
                    Nome = user.Nome,
                    Email = user.Email,
                    ImgUrl = user.ImgUrl
                };

                var response = await client.AddUserAsync(userDto);

                return response.Body.AddUserResult;
            });
        }
    }
}