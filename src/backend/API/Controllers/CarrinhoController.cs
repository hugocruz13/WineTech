using API.DTOs;
using API.Services;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

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

    }

}
