using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebVote.DataBase;
using WebVote.Entidades;
using WebVote.Models;

namespace WebVote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastrosController : ControllerBase
    {
        [Route("eleicao")]
        [HttpPost]
        public IActionResult PostInicial([FromBody] EleicoesModelSend candidatos)
        {
            if (!ModelState.IsValid) { return BadRequest(new ResponseModelSend { code = 400, message = "Estrutura do JSON esteja incorreta!" }); }
            ResponseModelSend response = ManipuladorInfo.InformacoesIniciais(candidatos);

            if (response.code == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);

        }

        [Route("eleicao/importacoes-secoes")]
        [HttpPost]
        public IActionResult PostInfo([FromBody] CandidatoModelSendList candidatos)
        {
                if (!ModelState.IsValid) { return BadRequest(new ResponseModelSend { code = 400, message = "Estrutura do JSON esteja incorreta!" }); }
            ResponseModelSend response = ManipuladorInfo.ImportaSecao(candidatos);

            if (response.code == 200)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
     
    }

}
