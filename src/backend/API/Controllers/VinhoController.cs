using API.DTOs;
using API.Services;
using BLL.Interfaces;
using BLL.Services;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinhoController : ControllerBase
    {
        private readonly IVinhoBLL _vinhoBLL;
        private readonly IStorageService _storageService;

        public VinhoController(IVinhoBLL vinhoBLL, IStorageService storageService)
        {
            _vinhoBLL = vinhoBLL;
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateVinhoDTO vinhoDTO)
        {
            try
            {
                Vinho vinho = new Vinho { Nome = vinhoDTO.Nome, Produtor = vinhoDTO.Produtor, Ano = vinhoDTO.Ano, Tipo = vinhoDTO.Tipo, Descricao = vinhoDTO.Descricao, Preco = vinhoDTO.Preco };
                vinho = await _vinhoBLL.InserirVinho(vinho);

                if (vinho.Id <= 0)
                    return StatusCode(500, new { success = false, message = "Erro ao inserir adega." });

                if (vinhoDTO.ImagemUrl != null && vinhoDTO.ImagemUrl.Length > 0)
                {
                    string imageUrl = await _storageService.UploadFileAsync(vinhoDTO.ImagemUrl, "vinho-images");
                    vinho.ImagemUrl = imageUrl;
                    await _vinhoBLL.ModificarVinho(vinho);
                }

                return Ok(new
                {
                    success = true,
                    message = "Vinho criado com sucesso.",
                    data = new
                    {
                        id = vinho.Id,
                        nome = vinho.Nome,
                        produtor = vinho.Produtor,
                        ano = vinho.Ano,
                        tipo = vinho.Tipo,
                        descricao = vinho.Descricao,
                        imageUrl = vinho.ImagemUrl,
                        preco = vinho.Preco
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
        public async Task<ActionResult> Put(int id, [FromBody] UpdateVinhoDTO vinhoDTO)
        {
            if (vinhoDTO.Id != 0 && vinhoDTO.Id != id)
                return BadRequest(new { success = false, message = "ID do URL difere do corpo." });

            try
            {
                Vinho vinho = new Vinho { Id = id, Nome = vinhoDTO.Nome, Produtor = vinhoDTO.Produtor, Ano = vinhoDTO.Ano, Tipo = vinhoDTO.Tipo, Descricao = vinhoDTO.Descricao, Preco = vinhoDTO.Preco };
                vinho = await _vinhoBLL.ModificarVinho(vinho);

                if (vinho == null)
                    return NotFound(new { success = false, message = "Vinho não encontrado." });

                return Ok(new
                {
                    success = true,
                    message = "Vinho atualizado com sucesso.",
                    data = new
                    {
                        id = vinho.Id,
                        nome = vinho.Nome,
                        produtor = vinho.Produtor,
                        ano = vinho.Ano,
                        tipo = vinho.Tipo,
                        descricao = vinho.Descricao,
                        imageUrl = vinho.ImagemUrl,
                        preco = vinho.Preco
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
        [HttpPost("{id}/upload-image")]
        public async Task<ActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "Arquivo inválido." });

            try
            {
                Vinho vinho = await _vinhoBLL.VinhoById(id);

                if (vinho == null)
                    return NotFound(new { success = false, message = "Vinho não encontrado." });

                string imageUrl = await _storageService.UploadFileAsync(file, "vinho-images");

                vinho.ImagemUrl = imageUrl;
                await _vinhoBLL.ModificarVinho(vinho);

                return Ok(new { success = true, message = "Imagem enviada com sucesso.", data = imageUrl });
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
                Vinho vinho = await _vinhoBLL.VinhoById(id);

                var data = new
                {
                    id = vinho.Id,
                    nome = vinho.Nome,
                    produtor = vinho.Produtor,
                    ano = vinho.Ano,
                    tipo = vinho.Tipo,
                    descricao = vinho.Descricao,
                    preco = vinho.Preco,
                    img = vinho.ImagemUrl
                };

                return Ok(new { success = true, data = data });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { success = false, message = "Vinho não encontrado." });
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
                List<Vinho> vinhos = await _vinhoBLL.TodosVinhos();
                var data = vinhos.Select(a => new
                {
                    id = a.Id,
                    nome = a.Nome,
                    produtor = a.Produtor,
                    ano = a.Ano,
                    tipo = a.Tipo,
                    descricao = a.Descricao,
                    preco = a.Preco
                }).ToList();

                return Ok(new { success = true, data = data });
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
                bool sucesso = await _vinhoBLL.ApagarVinho(id);
                if (!sucesso)
                    return NotFound(new { success = false, message = "Vinho não encontrado." });
                return Ok(new { success = true, message = "Vinho removido com sucesso." });
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
