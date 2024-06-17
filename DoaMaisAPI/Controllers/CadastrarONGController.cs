using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DoaMaisAPI.Azure;
using DoaMaisAPI.DAO;
using DoaMaisAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace DoaMaisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastrarONGController : ControllerBase
    {
        [HttpPost]
        [Route("cadastrarONG")]
        public IActionResult Cadastrar([FromBody] ONGDTO ong)
        {
            var dao = new ONGDAO();

            bool ongExiste = dao.VerificarONG(ong);
            if (ongExiste)
            {
                var mensagem = "E-mail já existe na base de dados";
                return Conflict(mensagem);
            }


            if (ong.Base64 is not null)
            {
                var azureBlobStorage = new AzureBlobStorage();
                ong.FotoPerfil = azureBlobStorage.UploadImage(ong.Base64);
            }

            dao.Cadastrar(ong);
            return Ok();
        }

        internal List<ONGDTO> Listar()
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = "SELECT * FROM ONGs";

            var comando = new MySqlCommand(query, conexao);

            var dataReader = comando.ExecuteReader();

            var ong = new List<ONGDTO>();

            while (dataReader.Read())
            {
                var ong1 = new ONGDTO();
                ong1.ID = int.Parse(dataReader["ID"].ToString());
                ong1.Email = dataReader["Email"].ToString();
                ong1.Senha = dataReader["Senha"].ToString();
                //ong.ImagemURL = dataReader["ImagemURL"].ToString();

                ong.Add(ong1);
            }
            conexao.Close();

            return ong;

        }

        [HttpPost]
        [Route("Loginong")]
        public IActionResult LoginONG([FromForm] ONGDTO ong)
        {
            var dao = new ONGDAO();
            var usuarioLogado = dao.Login(ong);

            if (usuarioLogado.ID == 0)
            {
                return NotFound("Usuário e/ou senha inválidos");
            }

            var token = GenerateJwtToken(usuarioLogado, "PU8a9W4sv2opkqlOwmgsn3w3Innlc4D5");
            return Ok(new { token });
        }

        private string GenerateJwtToken(ONGDTO ong, string secretKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("ID", ong.ID.ToString()),
                new Claim("Email", ong.Email),
            };

            var token = new JwtSecurityToken(
                "APIOngs",
                "APIOngs",
                claims,
                expires: DateTime.UtcNow.AddDays(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
