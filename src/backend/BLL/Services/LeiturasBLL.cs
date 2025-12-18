using DAL.Interfaces;
using DAL.Services;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace BLL.Services
{
    public class LeiturasBLL : ILeiturasBLL
    {
        private readonly ILeiturasDAL _leiturasDAL;
        private readonly IAdegaBLL _adegaBLL;

        public LeiturasBLL(ILeiturasDAL leiturasDAL, IAdegaBLL adegaBLL)
        {
            _leiturasDAL = leiturasDAL;
            _adegaBLL = adegaBLL;
        }
        public async Task<Leituras> InserirLeitura(Leituras leitura)
        {
            if (leitura == null)
                throw new ArgumentNullException(nameof(leitura));

            if (leitura.SensorId <= 0)
                throw new ArgumentException("SensorId inválido.", nameof(leitura.SensorId));

            if (leitura.Valor < 0)
                throw new ArgumentException("O valor da leitura não pode ser negativo.", nameof(leitura.Valor));

            return await _leiturasDAL.InserirLeitura(leitura);
        }
        public async Task<List<Leituras>> ObterLeiturasPorSensor(int sensorId)
        {
            if (sensorId <= 0)
                throw new ArgumentException("Sensor inválido.", nameof(sensorId));

            var leituras = await _leiturasDAL.ObterLeiturasPorSensor(sensorId);

            return leituras ?? new List<Leituras>();
        }

        public async Task<LeiturasStock> ObterUltimaLeituraPorSensor(int stockId)
        {
            if (stockId <= 0)
                throw new ArgumentException("Sensor inválido.", nameof(stockId));            

            return await _leiturasDAL.ObterLeiturasStock(stockId);
        }
    }
}
