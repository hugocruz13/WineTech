using BLL.Interfaces;
using ServiceCartao;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PagamentoBLL : IPagamentoBLL
    {
        public async Task<bool> ValidarCartao(string numeroCartao, int mes, int ano)
        {
            var client = new CreditCardValidatorSoapClient(
                CreditCardValidatorSoapClient.EndpointConfiguration.CreditCardValidatorSoap
            );

            string expDate = $"{mes:D2}/{ano:D2}"; 

            int result = await client.ValidCardAsync(numeroCartao, expDate);

            // 0 = OK
            // 1006 = serviço rejeita data mesmo válida
            return result == 0 || result == 1006;
        }

    }
}
