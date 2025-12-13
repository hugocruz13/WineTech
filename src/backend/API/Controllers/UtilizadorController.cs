using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorBLL _bll;

        public UtilizadorController(IUtilizadorBLL bll)
        {
            _bll = bll;
        }

        [HttpPost]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> RegisterUser()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                var email = User.FindFirst("email")?.Value;
                var nome = User.FindFirst("name")?.Value;
                var imgUrl = User.FindFirst("picture")?.Value;

                if (userId == null)
                    return BadRequest("Token inválido.");

                Utilizador user = await _bll.RegisterUserAsync(new Utilizador { Id = userId, Email = email, Nome = nome , ImgUrl = imgUrl });

                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno ao registar utilizador.", details = ex.Message });
            }
        }

        [HttpGet("me")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("sub")?.Value;
                if (userId == null)
                    return BadRequest("Token inválido.");

                var user = await _bll.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound("Usuário não encontrado.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno.", details = ex.Message });
            }
        }
    }
}