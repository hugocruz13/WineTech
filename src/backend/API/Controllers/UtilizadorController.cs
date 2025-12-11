using BLL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        [HttpPost("register")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> RegisterUser()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var userId = await _bll.RegisterUserAsync(accessToken);
                return Ok(new { Id = userId });
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
    }
}