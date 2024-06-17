using DoaMaisAPI.DAO;
using DoaMaisAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoaMaisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDoacaoController : ControllerBase
    {
        [HttpPost]
        public IActionResult CadastrarDoacao([FromBody] TipoDoacaoDTO TipoDeDoacao)
        {
            var dao = new TiposDoacoesDAO();
            dao.CadastrarDoacao(TipoDeDoacao);

            return Ok();
        }

        [HttpGet]
        [Route("listarPorID")]
        public IActionResult ListarDoacoesPorID(int id)
        {
            var dao = new ONGDAO();
            var ong = dao.ListarOngsPorID(id);

            return Ok(ong);

        }
    }
}
