using DoaMaisAPI.DTO;
using MySql.Data.MySqlClient;

namespace DoaMaisAPI.DAO
{
    public class TiposDoacoesDAO
    {
        internal void CadastrarDoacao(TipoDoacaoDTO Doacao)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO TiposDoacao (Nome) 
                        VALUES (@Nome);
                        SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@nome", Doacao.Nome);

            comando.ExecuteNonQuery();
            conexao.Close();
        }
    }
}
