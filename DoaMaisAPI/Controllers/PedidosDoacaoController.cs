using DoaMaisAPI.Azure;
using DoaMaisAPI.DAO;
using DoaMaisAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoaMaisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosDoacaoController : ControllerBase
    {
        //[HttpPost]
        //[Route("CadastrarPedidos")]
        //public IActionResult CadastrarPedidos(PedidoDoacaoDTO Pedidos)
        //{
        //    CadastrarUrls de imgs

        //    var azureBlobStorag = new AzureBlobStorage();

        //    foreach (var imagem in Pedidos.ImagensPedido)
        //    {
        //        var linkDaImagem = azureBlobStorag.UploadImage(imagem.Link);
        //        imagem.Link = linkDaImagem;
        //    }

        //    var statusPendente = new PedidoDoacaoDTO();
        //    statusPendente.ID = 1; // Tive que alterar por que a minha variavel status é bool

        //    Pedidos.Status = true;

        //    var dao = new PedidosDoacoesDAO();
        //    dao.CadastrarPedidos(Pedidos);

        //    return Ok();
        //}
        [HttpPost]
        [Route("cadastrarpedido")]
        public IActionResult CadastrarPedidos([FromBody] PedidoDoacaoDTO pedido)
        {


            var dao = new PedidosDoacoesDAO();

            if (pedido.ImagensPedido is not null)
            {
                foreach (var imagem in pedido.ImagensPedido)
                {
                    var azureBlobStorage = new AzureBlobStorage();
                    imagem.Link = azureBlobStorage.UploadImage(imagem.Base64);
                }
            }

            dao.CadastrarPedidos(pedido);
            return Ok();
        }


        [HttpGet]
        public IActionResult ListarPedidosDoacao()
        {
            var dao = new PedidosDoacoesDAO();
            var ONG = dao.ListarPedidosDoacao();

            return Ok(ONG);
        }
        private readonly PedidosDoacoesDAO _pedidosDoacoesDAO;

        public PedidosDoacaoController()
        {
            _pedidosDoacoesDAO = new PedidosDoacoesDAO();
        }

        [HttpPut("{id}/inativar")]
        public IActionResult InativarPedidoDoacao(int id)
        {
            try
            {
                _pedidosDoacoesDAO.InativarPedidoDoacao(id);
                return Ok(new { message = "Doação inativada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao inativar doação", error = ex.Message });
            }
        }
        [HttpPut("{id}/reativar")]
        public IActionResult ReativarPedidoDoacao(int id)
        {
            try
            {
                var pedidosDoacoesDAO = new PedidosDoacoesDAO();
                pedidosDoacoesDAO.ReativarPedidoDoacao(id);
                return Ok(new { message = "Doação reativada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao reativar doação", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("listarRequisicaoPorID")]
        public IActionResult ListarRequisicoesPorID(int id)
        {
            var dao = new PedidosDoacoesDAO();
            var ong = dao.ListarRequisicaoPorID(id);

            return Ok(ong);


        }
        [HttpGet]
        [Route("inativos/{ongId}")]
        public IActionResult ListarInativos([FromRoute] int ongId)
        {
            try
            {
                var dao = new PedidosDoacoesDAO();
                var pedidosInativos = dao.ListarInativos(ongId);
                return Ok(pedidosInativos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao listar pedidos de doação inativos", error = ex.Message });
            }
        }
        [HttpGet]
        [Route("ong/{ongId}")]
        public IActionResult ListarPedidosDoacaoPorONG([FromRoute]int ongId)
        {
            var dao = new PedidosDoacoesDAO();
            var pedidosPorONG = dao.ListarPedidosDoacaoPorONG(ongId);

            return Ok(pedidosPorONG);
        }
    }
}
