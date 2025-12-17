using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ISensoresBLL
    {
        Task<Models.Sensores> InserirSensor(Models.Sensores sensor);
        Task<List<Models.Sensores>> TodosSensores();
        Task<List<Models.Sensores>> ObterSensoresPorAdega(int adegaId);
        Task<List<Models.Leituras>> ObterLeiturasPorSensor(int sensorId);
    }
}
