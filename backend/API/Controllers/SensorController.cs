using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        //[HttpPost("data")]
        //public IActionResult Post([FromBody] SensorData data)
        //{
        //    if (data == null)
        //        return BadRequest("Dados inválidos");

        //    Console.WriteLine($"Recebido -> Temp: {data.Temperature}, Humi: {data.Humidity}, LDR: {data.LightIntensity}");

        //    return Ok("Dados recebidos com sucesso!");
        //}
    }
}
