using API.DTOs;
using API.Services;
using BLL.Interfaces;
using BLL.Services;
using DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorBLL _utilizadorbll;
        private readonly INotificacaoBLL _notificacaobll;
        private readonly IStorageService _storageService;

        public UtilizadorController(IUtilizadorBLL utilizadorbll, INotificacaoBLL notificacaoService, IStorageService storageService)
        {
            _utilizadorbll = utilizadorbll;
            _notificacaobll = notificacaoService;
            _storageService = storageService;
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


                var data = new
                {
                    id = user.Id,
                    nome = user.Nome,
                    email = user.Email,
                    imgUrl = user.ImgUrl
                };

                return Ok(data);
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

        [HttpPut("perfil")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> UpdatePerfil([FromForm] UtilizadorDTO utilizadorDTO)
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { success = false, message = "Utilizador não autenticado." });

                string imageUrl = null;

                if (utilizadorDTO.Imagem != null && utilizadorDTO.Imagem.Length > 0)
                {
                    imageUrl = await _storageService.UploadFileAsync(utilizadorDTO.Imagem, "user-images");
                }


                var input = new Utilizador { Id = userId, Nome = utilizadorDTO.Nome, Email = utilizadorDTO.Email, ImgUrl = imageUrl };
                var updated = await _utilizadorbll.UpdateUserAsync(input);
                if (updated == null)
                    return NotFound(new { success = false, message = "Utilizador não encontrado." });

                return Ok(new { success = true, data = new { id = updated.Id, nome = updated.Nome, email = updated.Email, imgUrl = updated.ImgUrl } });
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