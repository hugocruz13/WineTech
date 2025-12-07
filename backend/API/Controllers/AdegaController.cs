using Models;
using API.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdegaController : ControllerBase
    {
        private readonly IAdegaBLL _adegaBLL;

        public AdegaController(IAdegaBLL adegaBLL)
        {
            _adegaBLL = adegaBLL;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdegaDTO>>> Get()
        {
            try
            {
                List<Adega> adegas = await _adegaBLL.TodasAdegas();
                List<AdegaDTO> dtos = adegas.Select(a => new AdegaDTO { Id = a.Id, Localizacao = a.Localizacao }).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdegaDTO>> Get(int id)
        {
            try
            {
                Adega adega = await _adegaBLL.AdegaById(id);
                AdegaDTO dto = new AdegaDTO { Id = adega.Id, Localizacao = adega.Localizacao };
                return Ok(dto);
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Erro interno: {ex.Message}"); }
        }

        [HttpPost]
        public async Task<ActionResult<AdegaDTO>> Post([FromBody] AdegaDTO adegaDTO)
        {
            try
            {
                int id = await _adegaBLL.InserirAdega(adegaDTO.Localizacao);
                if (id <= 0)
                    return StatusCode(500, "Erro ao inserir adega.");

                adegaDTO.Id = id;
                return CreatedAtAction(nameof(Get), new { id = id }, adegaDTO);
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Erro interno: {ex.Message}"); }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AdegaDTO adegaDTO)
        {
            if (id != adegaDTO.Id)
                return BadRequest("ID do URL difere do corpo.");

            try
            {
                Adega adega = new Adega { Id = id, Localizacao = adegaDTO.Localizacao };
                bool sucesso = await _adegaBLL.ModificarAdega(adega);

                if (!sucesso)
                    return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Erro interno: {ex.Message}"); }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                bool sucesso = await _adegaBLL.ApagarAdega(id);
                if (!sucesso)
                    return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (Exception ex) { return StatusCode(500, $"Erro interno: {ex.Message}"); }
        }
    }
}
