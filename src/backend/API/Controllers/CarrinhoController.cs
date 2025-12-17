using API.DTOs;
using API.Services;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Reflection.PortableExecutable;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoBLL _carrinhoBLL;
        private readonly IStorageService _storageService;

        public CarrinhoController(ICarrinhoBLL carrinhoBLL, IStorageService storageService)
        {
            _carrinhoBLL = carrinhoBLL;
            _storageService = storageService;
        }

        [HttpGet]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> Get()
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            List<Carrinho> itens = await _carrinhoBLL.ObterCarrinhoPorUtilizador(sub);

            var data = itens.Select(c => new
            {
                id = c.Id,
                vinhosId = c.VinhosId,
                utilizadoresId = c.UtilizadoresId,
                quantidade = c.Quantidade
            }).ToList();

            return Ok(new { success = true, data = data });
        }

        [Authorize(Roles = "owner,user")]
        [HttpGet("detalhes")]
        public async Task<IActionResult> GetDetalhes()
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            List<CarrinhoDetalhe> itens = await _carrinhoBLL.ObterDetalhesCarrinho(sub);

            var data = itens.Select(c => new
            {
                VinhosId = c.VinhosId,
                NomeVinho = c.NomeVinho,
                Produtor = c.Produtor,
                Ano = c.Ano,
                Tipo = c.Tipo,
                Descricao = c.Descricao,
                ImagemUrl =c.ImagemUrl,
                Preco = c.Preco,
                Quantidade = c.Quantidade
            }).ToList();
            return Ok(new { success = true, data = data });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromQuery] InserirItemCarrinhoDTO dto)
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            try
            {
                var itemCarrinho = new Carrinho
                {
                    UtilizadoresId = sub,
                    VinhosId = dto.VinhosId,
                    Quantidade = dto.Quantidade
                };

                List<Carrinho> carrinhoAtualizado = await _carrinhoBLL.InserirItem(itemCarrinho);

                var data = carrinhoAtualizado.Select(c => new
                {
                    id = c.Id,
                    vinhosId = c.VinhosId,
                    utilizadoresId = c.UtilizadoresId,
                    quantidade = c.Quantidade
                }).ToList();

                return Ok(new
                {
                    success = true,
                    message = "Item adicionado ao carrinho com sucesso.",
                    data = data
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
        [HttpPut]
        public async Task<ActionResult> Put([FromQuery] InserirItemCarrinhoDTO dto)
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            try
            {
                var itemCarrinho = new Carrinho
                {
                    UtilizadoresId = sub,
                    VinhosId = dto.VinhosId,
                    Quantidade = dto.Quantidade 
                };

                List<Carrinho> carrinhoAtualizado = await _carrinhoBLL.AtualizarItem(itemCarrinho);

                var data = carrinhoAtualizado.Select(c => new
                {
                    id = c.Id,
                    vinhosId = c.VinhosId,
                    utilizadoresId = c.UtilizadoresId,
                    quantidade = c.Quantidade
                }).ToList();

                return Ok(new
                {
                    success = true,
                    message = $"Quantidade do vinho {dto.VinhosId} alterada para {dto.Quantidade}.",
                    data = data
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
        public async Task<ActionResult> Delete([FromQuery] int vinhoId, string utilizadoresId)
        {
            try
            {
                bool sucesso = await _carrinhoBLL.EliminarItem(vinhoId, utilizadoresId);
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
