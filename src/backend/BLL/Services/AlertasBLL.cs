using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AlertasBLL : IAlertasBLL
    {
        private readonly IAlertasDAL _alertasDAL;

        public  AlertasBLL(IAlertasDAL alertasDAL)
        {
            _alertasDAL = alertasDAL;
        }
        public async Task<Alertas> InserirAlerta(Alertas alerta)
        {
            if (alerta == null)
                throw new ArgumentNullException(nameof(alerta));

            if (alerta.SensoresId <= 0)
                throw new ArgumentException("SensoresId inválido.", nameof(alerta.SensoresId));

            if (string.IsNullOrWhiteSpace(alerta.TipoAlerta))
                throw new ArgumentException("O tipo de alerta é obrigatório.", nameof(alerta.TipoAlerta));

            if (string.IsNullOrWhiteSpace(alerta.Mensagem))
                throw new ArgumentException("A mensagem do alerta é obrigatória.", nameof(alerta.Mensagem));

            return await _alertasDAL.InserirAlerta(alerta);
        }
        public async Task<List<Alertas>> ObterAlertasPorSensor(int sensorId)
        {
            if (sensorId <= 0)
                throw new ArgumentException("Sensor inválido.", nameof(sensorId));

            var alertas = await _alertasDAL.ObterAlertasPorSensor(sensorId);

            return alertas ?? new List<Alertas>();
        }
    }
}
