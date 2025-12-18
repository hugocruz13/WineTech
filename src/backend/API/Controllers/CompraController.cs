using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ICompraBLL _compraBLL;

        public CompraController(ICompraBLL compraBLL)
        {
            _compraBLL = compraBLL;

        }

        [HttpPost]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> Post()
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            try
            {
                bool sucesso = await _compraBLL.ProcessarCarrinho(sub);

                if (!sucesso)
                    return BadRequest(new { success = false, message = "Não foi possível processar o carrinho." });

                return Ok(new { success = true, message = "Compra processada com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Erro interno: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> Get()
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            List<Compra> compras = await _compraBLL.ObterComprasPorUtilizador(sub);

            var data = compras.Select(a => new
            {
                idCompra = a.Id,
                utilizadorId = a.UtilizadorId,
                dataCompra = a.DataCompra,
                valorTotal = a.ValorTotal,
            }).ToList();

            return Ok(new { success = true, data = data });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> GetById(int id)
        {
            List<CompraDetalhe> compra = await _compraBLL.ObterCompraPorId(id);

            if (compra == null)
                return NotFound(new { success = false, message = "Compra não encontrada." });

            var data = compra.Select(a => new
            {
                idCompra = a.IdCompra,
                valorTotal = a.ValorTotal,
                dataCompra = a.DataCompra,
                idVinho = a.IdVinho,
                nome = a.Nome,
                produtor = a.Produtor,
                ano = a.Ano,
                tipo = a.Tipo,
                quantidade = a.Quantidade,
                preco = a.Preco,
                imgVinho = a.ImgVinho,
                nomeUtilizador = a.NomeUtilizador,
                emailUtilizador = a.EmailUtilizador,
                imagemUtilizador = a.ImagemUtilizador,
                stockId = a.StockId
            }).ToList();

            return Ok(new { success = true, data = data });
        }
    }
}
