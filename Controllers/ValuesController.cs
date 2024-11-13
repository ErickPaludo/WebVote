using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebVote.Entidades;

namespace WebVote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet(Name = "Apuracao")]
        public IActionResult Get()
        {
            var candidatos = new Candidatos
            {
                candidatos = new List<People>  // Inicializa a lista de `People`
        {
            new People { nome = "Mario", votos = 12 }
        }
            };
            return Ok(candidatos);
        }
    }
}
