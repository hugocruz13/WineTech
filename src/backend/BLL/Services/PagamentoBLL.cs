using BLL.Interfaces;
using ServiceCartao;
using System;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PagamentoBLL : IPagamentoBLL
    {
        public async Task<bool> ValidarCartao(string numeroCartao, int mes, int ano)
        {
            if (!ValidadeMaiorQueHoje(mes, ano))
                return false;

            var client = new CreditCardValidatorSoapClient(
                CreditCardValidatorSoapClient.EndpointConfiguration.CreditCardValidatorSoap
            );

            try
            {
                string type = await client.GetCardTypeAsync(numeroCartao);

                if (string.IsNullOrWhiteSpace(type) || type == "UNKNOWN")
                    return false;

                return true;
            }
            finally
            {
                if (client.State == System.ServiceModel.CommunicationState.Faulted)
                    client.Abort();
                else
                    client.Close();
            }
        }

        private bool ValidadeMaiorQueHoje(int mes, int ano)
        {
            if (ano < 100)
                ano += 2000;

            var ultimoDiaMes = DateTime.DaysInMonth(ano, mes);

            var dataValidade = new DateTime(
                ano,
                mes,
                ultimoDiaMes,
                23, 59, 59
            );

            return dataValidade > DateTime.Now;
        }

    }
}
