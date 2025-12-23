using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LeiturasBLL : ILeiturasBLL
    {
        private readonly ILeiturasDAL _leiturasDAL;
        private readonly IAdegaBLL _adegaBLL;
        private readonly IAlertasDAL _alertasDAL;
        private readonly ISensoresDAL _sensoresDAL;
        private readonly IUtilizadorDAL _utilizadoresDAL;
        private readonly ILeituraRealtimeService _realtime;
        private readonly INotificacaoBLL _notificacaoBLL;

        public LeiturasBLL(ILeiturasDAL leiturasDAL, IAdegaBLL adegaBLL, IAlertasDAL alertasDAL, ISensoresDAL sensoresDAL, ILeituraRealtimeService realtime, IUtilizadorDAL utilizadorDAL, INotificacaoBLL notificacaoBLL)
        {
            _leiturasDAL = leiturasDAL;
            _adegaBLL = adegaBLL;
            _alertasDAL = alertasDAL;
            _sensoresDAL = sensoresDAL;
            _realtime = realtime;
            _utilizadoresDAL = utilizadorDAL;
            _notificacaoBLL = notificacaoBLL;
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
            novaLeitura.Tipo =  leitura.Tipo;
            novaLeitura.AdegaId = leitura.AdegaId;

            if (novaLeitura != null)
            {
                await VerificarAlerta(leitura);
            }

            List<Utilizador> owners = await _utilizadoresDAL.GetOwnersAsync();

            foreach (var owner in owners)
            {
                await _realtime.SendToUserAsync(owner.Id, novaLeitura);
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

        public async Task<LeiturasStock> ObterLeituraPorAdega(int adegaId)
        {
            if (adegaId <= 0)
                throw new ArgumentException("Adega inválida.", nameof(adegaId));

            return await _leiturasDAL.ObterLeiturasAdega(adegaId);
        }

        private async Task VerificarAlerta(Leituras leitura)
        {

            var sensor = await _sensoresDAL.TodosSensores();
            if (sensor == null || !sensor.Any(s => s.Id == leitura.SensorId))
                return;

            string tipoSensor = sensor.First(s => s.Id == leitura.SensorId).Tipo;

            var todasLeituras = await _leiturasDAL.ObterLeiturasPorSensor(leitura.SensorId);
            var ultimas3 = todasLeituras.Take(3).ToList();

            if (ultimas3.Count < 3)
                return;

            List<Utilizador> owners = await _utilizadoresDAL.GetOwnersAsync();

            float min = 0, max = 0;

            switch (leitura.Tipo)
            {
                case "Temperatura":
                    min = 15f;
                    max = 18f;
                    break;

                case "Humidade":
                    min = 60f;
                    max = 80f;
                    break;

                case "Luminosidade":
                    min = 0f;
                    max = 201f;
                    break;

                default:
                    return;
            }

            bool sistemaInstavel = ultimas3.All(l => l.Valor < min || l.Valor > max);

            if (sistemaInstavel)
            {
                var novoAlerta = new Models.Alertas
                {
                    SensoresId = leitura.SensorId,
                    TipoAlerta = "Instabilidade Crítica",
                    Mensagem = $"As últimas 3 leituras estão fora da zona segura ({min}-{max}).",
                };

                await _alertasDAL.InserirAlerta(novoAlerta);

                foreach (var owner in owners)
                {
                    await _notificacaoBLL.InserirNotificacao(new Notificacao { Titulo = "Alerta de Sensor", Mensagem = $"O sensor {tipoSensor} registou valores fora do intervalo seguro.", Tipo = TipoNotificacao.Alerta, UtilizadorId = owner.Id });
                }
            }
        }
    }
}
