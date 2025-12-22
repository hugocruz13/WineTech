using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILeiturasBLL
    {
        Task<Models.Leituras> InserirLeitura(Models.Leituras leitura);
        Task<List<Models.Leituras>> ObterLeiturasPorSensor(int sensorId);
        Task<LeiturasStock> ObterUltimaLeituraPorSensor(int sensorId);
    }
}
