using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertasBLL _alertasBLL;

        public AlertasController(IAlertasBLL alertasBLL)
        {
            _alertasBLL = alertasBLL;
        }

        [HttpGet("{sensorId}/alertas")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> GetAlertasPorSensor(int sensorId)
        {
            try
            {
                if (sensorId <= 0)
                    return BadRequest(new { success = false, message = "Sensor inválido." });

                List<Models.Alertas> alertas = await _alertasBLL.ObterAlertasPorSensor(sensorId);

                var data = alertas.Select(a => new
                {
                    id = a.Id,
                    sensoresId = a.SensoresId,
                    tipoAlerta = a.TipoAlerta,
                    mensagem = a.Mensagem,
                    dataHora = a.DataHora,
                    resolvido = a.Resolvido
                }).ToList();

                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

        [HttpGet("todos")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> GetAllAlertas()
        {
            try
            {
                var alertas = await _alertasBLL.GetAllAlertas();

                var data = alertas.Select(a => new
                {
                    id = a.Id,
                    tipoAlerta = a.TipoAlerta,
                    mensagem = a.Mensagem,
                    dataHora = a.DataHora,
                    resolvido = a.Resolvido,
                    sensoresId = a.SensoresId,
                    identificadorHardware = a.IdentificadorHardware,
                    tipoSensor = a.TipoSensor,
                    adegaId = a.AdegaId
                }).ToList();

                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

        [HttpPut("{alertaId}/resolver")]
        [Authorize(Roles = "owner")]
        public async Task<IActionResult> ResolverAlerta(int alertaId)
        {
            try
            {
                var result = await _alertasBLL.ResolverAlerta(alertaId);
                return Ok(new { success = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }
    }
}
