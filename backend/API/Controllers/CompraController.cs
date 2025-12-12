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
            var accessToken = await HttpContext.GetTokenAsync("access_token");
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

    }
}
