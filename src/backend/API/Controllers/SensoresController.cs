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
    public class SensoresController : ControllerBase
    {
        private readonly ISensoresBLL _sensoresBLL;
        private readonly IStorageService _storageService;

        public SensoresController(ISensoresBLL sensoresBLL, IStorageService storageService)
        {
            _sensoresBLL = sensoresBLL;
            _storageService = storageService;
        }
        [HttpPost]
        [Authorize(Roles = "owner")]
        public async Task<ActionResult> Post([FromForm] InserirSensorDTO dto)
        {
            try
            {
                string? imageUrl = null;

                if (dto.ImagemUrl != null && dto.ImagemUrl.Length > 0)
                {
                    imageUrl = await _storageService.UploadFileAsync(dto.ImagemUrl, "sensor-images");
                }

                var sensor = new Models.Sensores
                {
                    IdentificadorHardware = dto.IdentificadorHardware,
                    Tipo = dto.Tipo,
                    Estado = dto.Estado,
                    ImagemUrl = imageUrl,
                    AdegaId = dto.AdegaId
                };

                Models.Sensores sensorCriado = await _sensoresBLL.InserirSensor(sensor);

                if (sensorCriado == null)
                    return StatusCode(500, new { success = false, message = "Erro ao inserir sensor." });

                return Ok(new
                {
                    success = true,
                    message = "Sensor criado com sucesso.",
                    data = new
                    {
                        id = sensorCriado.Id,
                        identificadorHardware = sensorCriado.IdentificadorHardware,
                        tipo = sensorCriado.Tipo,
                        estado = sensorCriado.Estado,
                        imagemUrl = sensorCriado.ImagemUrl,
                        adegaId = sensorCriado.AdegaId
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
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                List<Models.Sensores> sensores = await _sensoresBLL.TodosSensores();
                var data = sensores.Select(a => new
                {
                    id = a.Id,
                    identificadorHardware = a.IdentificadorHardware,
                    tipo = a.Tipo,
                    estado = a.Estado,
                    imagemUrl = a.ImagemUrl,
                    adegaId = a.AdegaId,
                }).ToList();

                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

        [HttpGet("{adegaId}")]
        public async Task<IActionResult> Get(int adegaId)
        {
            try
            {
                if (adegaId <= 0)
                    return BadRequest(new { success = false, message = "Adega inválida." });

                List<Models.Sensores> sensores = await _sensoresBLL.ObterSensoresPorAdega(adegaId);

                var data = sensores.Select(s => new
                {
                    id = s.Id,
                    identificadorHardware = s.IdentificadorHardware,
                    tipo = s.Tipo,
                    estado = s.Estado,
                    imagemUrl = s.ImagemUrl,
                    adegaId = s.AdegaId
                }).ToList();

                return Ok(new { success = true, data = data });
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

                List<Models.Leituras> leituras = await _sensoresBLL.ObterLeiturasPorSensor(sensorId);

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

    }
}
