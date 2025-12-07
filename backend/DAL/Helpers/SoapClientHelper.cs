using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace DAL.Helpers
{
    public static class SoapClientHelper
    {
        public static async Task<TResult> ExecuteAsync<TClient, TResult>(
            Func<TClient> clientFactory,
            Func<TClient, Task<TResult>> action)
            where TClient : ICommunicationObject
        {
            var client = clientFactory();
            try
            {
                var result = await action(client);
                client.Close();
                return result;
            }
            catch (Exception)
            {
                if (client != null)
                {
                    client.Abort();
                }
                throw;
            }
        }
    }
}