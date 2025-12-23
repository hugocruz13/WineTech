using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAlertasDAL
    {
        Task<Models.Alertas> InserirAlerta(Models.Alertas alerta);

        Task<List<Models.Alertas>> ObterAlertasPorSensor(int sensorId);
        Task<List<Models.AlertaComSensor>> GetAllAlertas();
        Task<bool> ResolverAlerta(int alertaId);
    }
}
