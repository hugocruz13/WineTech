using API.DTOs;
using API.Services;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("detalhes")]
        [Authorize(Roles = "owner,user")]
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
                ImagemUrl = c.ImagemUrl,
                Preco = c.Preco,
                Quantidade = c.Quantidade
            }).ToList();
            return Ok(new { success = true, data = data });
        }

        [HttpPost]
        [Authorize(Roles = "owner,user")]
        public async Task<ActionResult> Post([FromBody] InserirItemCarrinhoDTO dto)
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

                List<Carrinho> carrinhoAtualizado = await _carrinhoBLL.InserirItem(itemCarrinho, sub);

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

        [HttpPut("add")]
        [Authorize(Roles = "owner,user")]
        public async Task<ActionResult> Put([FromBody] InserirItemCarrinhoDTO dto)
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

                List<Carrinho> carrinhoAtualizado = await _carrinhoBLL.AumentarItemCarrinho(itemCarrinho, sub);

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

        [HttpPut("dec")]
        [Authorize(Roles = "owner,user")]
        public async Task<ActionResult> PutDiminuit([FromBody] InserirItemCarrinhoDTO dto)
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

                List<Carrinho> carrinhoAtualizado = await _carrinhoBLL.DiminuirItemCarrinho(itemCarrinho,sub);

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

        [HttpDelete]
        [Authorize(Roles = "owner,user")]
        public async Task<ActionResult> Delete([FromQuery] int vinhoId)
        {
            try
            {
                var sub = User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(sub))
                    return Unauthorized(new { success = false, message = "Utilizador não autenticado." });


                bool sucesso = await _carrinhoBLL.EliminarItem(vinhoId, sub);
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
