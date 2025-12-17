using BLL.Interfaces;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorBLL _utilizadorbll;
        private readonly INotificacaoBLL _notificacaobll;

        public UtilizadorController(IUtilizadorBLL utilizadorbll, INotificacaoBLL notificacaoService)
        {
            _utilizadorbll = utilizadorbll;
            _notificacaobll = notificacaoService;
        }

        [HttpGet("perfil")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (userId == null)
                    return BadRequest("Token inválido.");

                var user = await _utilizadorbll.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno.", details = ex.Message });
            }
        }


        [HttpGet("notificacoes")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> Get()
        {
            var sub = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(sub))
                return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

            var data = _notificacaobll.ObterNotificacoesPorUtilizador(sub);

            return Ok(new { success = true, data = data.Result });
        }

        [HttpPut("notificacoes/{id}/lida")]
        public async Task<IActionResult> MarcarNotificacaoComoLida(int id)
        {
            var result = await _notificacaobll.MarcarNotificacaoComoLida(id);
            if (result == null)
                return NotFound(new { success = false, message = "Notificação não encontrada." });
            return Ok(new { success = true, data = result });
        }

        [Authorize]
        [HttpGet("notificacoes/teste")]
        public async Task<IActionResult> GetNotificacoes()
        {
            var sub = User.FindFirst("sub")?.Value;

            await _notificacaobll.NotificacaoTesteParaUtilizador(sub);
            return Ok("Notificação enviada");
        }
    }
}