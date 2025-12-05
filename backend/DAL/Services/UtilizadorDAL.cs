using DAL.Interfaces;
using ServiceUtilizador;
using System.Threading.Tasks;
using Utilizador = Models.Utilizador;

namespace DAL.Services
{
    public class UtilizadorDAL : IUtilizadorDAL
    {
        public async Task<int> AddUserAsync(Utilizador user)
        {
            var client = new UtilizadorRepositoryServiceSoapClient(UtilizadorRepositoryServiceSoapClient.EndpointConfiguration.UtilizadorRepositoryServiceSoap);
            var userDto = new ServiceUtilizador.Utilizador
            {
                Auth0UserId = user.Auth0UserId,
                Nome = user.Nome,
                Email = user.Email,
                ImgUrl = user.ImgUrl
            };

            try
            {
                var response = await client.AddUserAsync(userDto);
                await client.CloseAsync();
                return response.Body.AddUserResult;

            }
            catch
            {
                client.Abort();
                throw;
            }
        }
    }
}
