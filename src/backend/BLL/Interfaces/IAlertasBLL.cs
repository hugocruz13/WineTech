using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAlertasBLL
    {
        Task<Models.Alertas> InserirAlerta(Models.Alertas alerta);
        Task<List<Models.Alertas>> ObterAlertasPorSensor(int sensorId);
    }
}
