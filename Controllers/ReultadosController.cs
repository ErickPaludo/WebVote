using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebVote.DataBase;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReultadosController : ControllerBase
    {
        [Route("eleicao/importacoes-secoes")]
        [HttpGet]
        public ResponseSecoes PostInicial(int zonaid, int secaoid)
        {
           // if (!ModelState.IsValid) { return BadRequest(new ResponseModelSend { code = 400, message = "Estrutura do JSON esteja incorreta!" }); }
            return OracleDb.RetornoSecoes(zonaid, secaoid);
        }
        [Route("eleicao/resultados")]
        [HttpGet]
        public IActionResult PostInfo([FromBody] CandidatoModelSendList candidatos)
        {
            if (!ModelState.IsValid) { return BadRequest(new ResponseModelSend { code = 400, message = "Estrutura do JSON esteja incorreta!" }); }
            return Ok(new ResponseModelSend { code = 200, message = "Cadastrado com sucesso!" });
        }
    }
}
