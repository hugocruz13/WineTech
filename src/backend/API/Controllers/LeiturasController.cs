using API.DTOs;
using API.Services;
using BLL.Interfaces;
using BLL.Services;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Reflection.PortableExecutable;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeiturasController : ControllerBase
    {
        private readonly ILeiturasBLL _leiturasBLL;

        public LeiturasController(ILeiturasBLL leiturasBLL)
        {
            _leiturasBLL = leiturasBLL;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InserirLeituraDTO dto)
        {
            try
            {
                var leitura = new Models.Leituras{ SensorId = dto.SensorId, Valor = dto.Valor};

                Models.Leituras leituraCriada = await _leiturasBLL.InserirLeitura(leitura);

                if (leituraCriada == null)
                    return StatusCode(500, new { success = false, message = "Erro ao inserir leitura." });

                return Ok(new
                {
                    success = true,
                    message = "Leitura registada com sucesso.",
                    data = new
                    {
                        id = leituraCriada.Id,
                        sensorId = leituraCriada.SensorId,
                        valor = leituraCriada.Valor,
                        dataHora = leituraCriada.DataHora 
                    }
                });
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
        [HttpGet("{sensorId}/leituras")]
        public async Task<IActionResult> GetLeiturasPorSensor(int sensorId)
        {
            try
            {
                if (sensorId <= 0)
                    return BadRequest(new { success = false, message = "Sensor inválido." });

                List<Models.Leituras> leituras = await _leiturasBLL.ObterLeiturasPorSensor(sensorId);

                var data = leituras.Select(l => new
                {
                    id = l.Id,
                    sensorId = l.SensorId,
                    valor = l.Valor,
                    dataHora = l.DataHora
                }).ToList();

                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

        [HttpGet("{stockId}/leituras/stock")]
        public async Task<IActionResult> GetUltimaLeituraPorSensor(int stockId)
        {
            try
            {
                if (stockId <= 0)
                    return BadRequest(new { success = false, message = "Sensor inválido." });

                LeiturasStock leiturasStock = await _leiturasBLL.ObterUltimaLeituraPorSensor(stockId);
                return Ok(new { success = true, data = leiturasStock });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }
    }
}
