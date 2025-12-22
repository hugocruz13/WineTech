using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ILeiturasDAL
    {
        Task<Models.Leituras> InserirLeitura(Models.Leituras leitura);
        Task<List<Models.Leituras>> ObterLeiturasPorSensor(int sensorId);
        Task<Models.LeiturasStock> ObterLeiturasStock(int stockId);
        Task<Models.LeiturasStock> ObterLeiturasAdega(int adegaId);
    }
}
