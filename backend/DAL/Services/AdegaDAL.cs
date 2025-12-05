using Models;
using DAL.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;


namespace DAL.Services
{
    public class AdegaDAL : IAdegaDAL
    {
        public async Task<int> InserirAdega(string localizacao) 
        {
            var cliente = new ServiceAdega.AdegaRepositoryServiceSoapClient(ServiceAdega.AdegaRepositoryServiceSoapClient.EndpointConfiguration.AdegaRepositoryServiceSoap);

            try
            {
                var response = await cliente.InserirAdegaAsync(localizacao);
                await cliente.CloseAsync();
                return 1;
            }
            catch 
            {
                return -1;
            }
        }

        public List<Adega> TodasAdegas() 
        {
            return null;
        }

        public Adega AdegaById(int id) 
        {
            return null;
        }

        public bool ModificarAdega(Adega adega) 
        {
            return false;
        }

        public bool ApagarAdega(int id) 
        {
            return false;
        }
    }
}
