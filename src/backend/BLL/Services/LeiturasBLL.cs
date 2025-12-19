using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeiturasBLL : ILeiturasBLL
    {
        private readonly ILeiturasDAL _leiturasDAL;
        private readonly IAdegaBLL _adegaBLL;
        private readonly IAlertasDAL _alertasDAL;
        private readonly ISensoresDAL _sensoresDAL;
        public LeiturasBLL(ILeiturasDAL leiturasDAL, IAdegaBLL adegaBLL, IAlertasDAL alertasDAL, ISensoresDAL sensoresDAL)
        {
            _leiturasDAL = leiturasDAL;
            _adegaBLL = adegaBLL;
            _alertasDAL = alertasDAL;
            _sensoresDAL = sensoresDAL;
        }
        public async Task<Leituras> InserirLeitura(Leituras leitura)
        {
            if (leitura == null)
                throw new ArgumentNullException(nameof(leitura));

            if (leitura.SensorId <= 0)
                throw new ArgumentException("SensorId inválido.", nameof(leitura.SensorId));

            if (leitura.Valor < 0)
                throw new ArgumentException("O valor da leitura não pode ser negativo.", nameof(leitura.Valor));
            
            var novaLeitura = await _leiturasDAL.InserirLeitura(leitura);
            
            if (novaLeitura != null)
            {
                await VerificarAlerta(novaLeitura.SensorId);
            }
            return novaLeitura;
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
        private async Task VerificarAlerta(int sensorId)
        {
            var todasLeituras = await _leiturasDAL.ObterLeiturasPorSensor(sensorId);
            var ultimas3 = todasLeituras.Take(3).ToList();

            if (ultimas3.Count < 3) return;

            float min = 0, max = 0;

            switch (sensorId)
            {
                case 1: (min, max) = (15f, 30f); break; 
                case 2: (min, max) = (50f, 80f); break; 
                case 3: (min, max) = (0f, 1000f); break; 
                default: return; 
            }

            bool sistemaInstavel = ultimas3.All(l => l.Valor < min || l.Valor > max);

            if (sistemaInstavel)
            {
                var novoAlerta = new Models.Alertas
                {
                    SensoresId = sensorId,
                    TipoAlerta = "Instabilidade Crítica",
                    Mensagem = $"As últimas 3 leituras estão fora da zona segura ({min}-{max}).",
                };

                await _alertasDAL.InserirAlerta(novoAlerta);
            }
        }
    }
}
