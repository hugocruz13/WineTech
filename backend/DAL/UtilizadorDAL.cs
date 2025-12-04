using System;
using System.Threading.Tasks;
using ServiceReference1;

namespace DAL
{
    public class UtilizadorDAL
    {
        public async Task InsertUserAsync(string auth0UserId)
        {
            var client = new UtilizadorRepositorySoapClient(UtilizadorRepositorySoapClient.EndpointConfiguration.UtilizadorRepositorySoap);

            var user = new Utilizador
            {
                Auth0UserId = auth0UserId
            };

            await client.InsertUserAsync(user);
            await client.CloseAsync();
        }
    }
}   