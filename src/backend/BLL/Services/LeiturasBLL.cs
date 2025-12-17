using DAL.Interfaces;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeiturasBLL
    {
        private readonly ILeiturasDAL _leiturasDAL;

        public LeiturasBLL(ILeiturasDAL leiturasDAL)
        {
            _leiturasDAL = leiturasDAL;
        }
        public async Task<Models.Leituras> InserirLeitura(Models.Leituras leitura)
        {
            if (leitura == null)
                throw new ArgumentNullException(nameof(leitura));

            if (leitura.SensorId <= 0)
                throw new ArgumentException("SensorId inválido.", nameof(leitura.SensorId));

            if (leitura.Valor < 0)
                throw new ArgumentException("O valor da leitura não pode ser negativo.", nameof(leitura.Valor));

            return await _leiturasDAL.InserirLeitura(leitura);
        }
        public async Task<List<Models.Leituras>> ObterLeiturasPorSensor(int sensorId)
        {
            if (sensorId <= 0)
                throw new ArgumentException("Sensor inválido.", nameof(sensorId));

            var leituras = await _leiturasDAL.ObterLeiturasPorSensor(sensorId);

            return leituras ?? new List<Models.Leituras>();
        }
    }
}
