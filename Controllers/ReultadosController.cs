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
        public IActionResult PostInicial(int zonaid, int secaoid)
        {
            var obj = OracleDb.RetornoSecoes(zonaid, secaoid) ;
            if (!string.IsNullOrEmpty(obj.nomeEleicao))
            {
                return Ok(obj);
            }
            {
                return BadRequest(new ResponseModelSend { code = 400, message = "Nenhuma seção encontrada!" });
            }
        }
        [Route("eleicao/resultados")]
        [HttpGet]
        public IActionResult PostInfo(int zonaid, int secaoid)
        {
            var obj = OracleDb.RetornoFinal(zonaid, secaoid);
            if (!string.IsNullOrEmpty(obj.nomeEleicao))
            {
                return Ok(obj);
            }
            else
            {
                return BadRequest(new ResponseModelSend { code = 400, message = "Nenhuma eleição cadastrado!" });
            }
        }
    }
}
