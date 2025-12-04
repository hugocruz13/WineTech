using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        [HttpPost("insert")]
        [Authorize(Roles = "owner,user")]
        public async Task<IActionResult> Insert()
        {
            var subClaim = User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(subClaim))
                return BadRequest("User ID claim not found.");

            var bll = new UtilizadorBLL();
            await bll.InserteUserAsync(subClaim);

            return Ok();
        }
    }
}