using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebVote.DataBase;
using WebVote.Models;

namespace WebVote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrachaController : ControllerBase
    {
        [Route("eleicao/deletartudo")]
        [HttpDelete]
        public IActionResult DeleteAll()
        {
            OracleDb.DeleteAll();
            return Ok(new ResponseModelSend { code = 200, message = "Fez merda mano, deletou foi tudo :c" });
        }
    }
}
