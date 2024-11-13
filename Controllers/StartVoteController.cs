using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebVote.Entidades;

namespace WebVote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StartVoteController : ControllerBase
    {
        [HttpPost(Name = "ObterCandidato")]
        public IActionResult Get([FromBody]  Candidatos candidatos)
        {
            foreach (var obj in candidatos.candidatos) {
                if (string.IsNullOrEmpty(obj.nome)) {
                    return BadRequest("O campo Nome deve estar preenchido!");
                }
            }
            return Ok("Cadastro recebido!");
        }
    }
}
