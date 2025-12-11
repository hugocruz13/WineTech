using API.DTOs;
using API.Services;
using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdegaController : ControllerBase
    {
        private readonly IAdegaBLL _adegaBLL;
        private readonly IStorageService _storageService;

        public AdegaController(IAdegaBLL adegaBLL, IStorageService storageService)
        {
            _adegaBLL = adegaBLL;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                List<Adega> adegas = await _adegaBLL.TodasAdegas();
                var data = adegas.Select(a => new
                {
                    id = a.Id,
                    nome = a.Nome,
                    localizacao = a.Localizacao,
                    capacidade = a.Capacidade,
                    imageUrl = a.ImagemUrl,
                }).ToList();

                return Ok(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                Adega adega = await _adegaBLL.AdegaById(id);

                var data = new
                {
                    id = adega.Id,
                    nome = adega.Nome,
                    localizacao = adega.Localizacao,
                    capacidade = adega.Capacidade,
                    imageUrl = adega.ImagemUrl,
                    vinhos = adega.Vinhos
                };

                return Ok(new { success = true, data = data });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { success = false, message = "Adega não encontrada." });
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

        [HttpPost]
        public async Task<ActionResult> Post(CreateAdegaDTO adegaDTO)
        {
            try
            {
                Adega adega = new Adega { Nome = adegaDTO.Nome, Localizacao = adegaDTO.Localizacao, Capacidade = adegaDTO.Capacidade, };
                adega = await _adegaBLL.InserirAdega(adega);

                if (adega.Id <= 0)
                    return StatusCode(500, new { success = false, message = "Erro ao inserir adega." });

                if (adegaDTO.Imagem != null && adegaDTO.Imagem.Length > 0)
                {
                    string imageUrl = await _storageService.UploadFileAsync(adegaDTO.Imagem, "adega-images");
                    adega.ImagemUrl = imageUrl;
                    await _adegaBLL.ModificarAdega(adega);
                }

                return Ok(new
                {
                    success = true,
                    message = "Adega criada com sucesso.",
                    data = new
                    {
                        id = adega.Id,
                        nome = adega.Nome,
                        localizacao = adega.Localizacao,
                        capacidade = adega.Capacidade,
                        imageUrl = adega.ImagemUrl,
                        vinhos = new List<Vinho>() 
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


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateAdegaDTO adegaDTO)
        {
            if (adegaDTO.Id != 0 && adegaDTO.Id != id)
                return BadRequest(new { success = false, message = "ID do URL difere do corpo." });

            try
            {
                Adega adega = new Adega { Id = id, Nome = adegaDTO.Nome, Localizacao = adegaDTO.Localizacao, Capacidade = adegaDTO.Capacidade };
                adega = await _adegaBLL.ModificarAdega(adega);

                if (adega == null)
                    return NotFound(new { success = false, message = "Adega não encontrada." });

                return Ok(new
                {
                    success = true,
                    message = "Adega atualizada com sucesso.",
                    data = new
                    {
                        id = adega.Id,
                        nome = adega.Nome,
                        localizacao = adega.Localizacao,
                        capacidade = adega.Capacidade,
                        imageUrl = adega.ImagemUrl,
                        vinhos = adega.Vinhos
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



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                bool sucesso = await _adegaBLL.ApagarAdega(id);
                if (!sucesso)
                    return NotFound(new { success = false, message = "Adega não encontrada." });
                return Ok(new { success = true, message = "Adega removida com sucesso." });
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

        [HttpPost("{id}/upload-image")]
        public async Task<ActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "Arquivo inválido." });

            try
            {
                Adega adega = await _adegaBLL.AdegaById(id);

                if (adega == null)
                    return NotFound(new { success = false, message = "Adega não encontrada." });

                string imageUrl = await _storageService.UploadFileAsync(file, "adega-images");

                adega.ImagemUrl = imageUrl;
                await _adegaBLL.ModificarAdega(adega);

                return Ok(new { success = true, message = "Imagem enviada com sucesso.", data = imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

    }
}
