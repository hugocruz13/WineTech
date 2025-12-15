using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno.", details = ex.Message });
            }
        }
    }
}