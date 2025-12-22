using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class SensoresBLL : ISensoresBLL
    {
        private readonly ISensoresDAL _sensoresDAL;

        public SensoresBLL(ISensoresDAL sensoresDAL)
        {
            _sensoresDAL = sensoresDAL;
        }

        public async Task<Models.Sensores> InserirSensor(Models.Sensores sensor)
        {
            if (sensor == null)
                throw new ArgumentNullException(nameof(sensor));

            if (string.IsNullOrWhiteSpace(sensor.IdentificadorHardware))
                throw new ArgumentException("IdentificadorHardware inválido.", nameof(sensor.IdentificadorHardware));

            if (string.IsNullOrWhiteSpace(sensor.Tipo))
                throw new ArgumentException("Tipo inválido.", nameof(sensor.Tipo));

            if (sensor.AdegaId <= 0)
                throw new ArgumentException("AdegaId inválido.", nameof(sensor.AdegaId));

            return await _sensoresDAL.InserirSensor(sensor);
        }
        public async Task<List<Models.Sensores>> TodosSensores()
        {
            var sensores = await _sensoresDAL.TodosSensores();

            return sensores ?? new List<Models.Sensores>();
        }
        public async Task<List<Models.Sensores>> ObterSensoresPorAdega(int adegaId)
        {
            if (adegaId <= 0)
                throw new ArgumentException("Adega inválida.", nameof(adegaId));

            var sensores = await _sensoresDAL.ObterSensoresPorAdega(adegaId);

            return sensores ?? new List<Models.Sensores>();
        }
    }
}
