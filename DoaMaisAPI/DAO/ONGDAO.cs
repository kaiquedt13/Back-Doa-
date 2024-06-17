using DoaMaisAPI.DTO;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace DoaMaisAPI.DAO
{
    public class ONGDAO
    {
        internal void Cadastrar(ONGDTO ong)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();
            var query = @"INSERT INTO ONGs (Nome, Celular, Email, Senha, Cep, Logradouro, Numero, Cidade, Bairro, Complemento, Estado, FotoPerfil, Biografia) 
                        VALUES (@nome, @celular, @email, @senha, @cep, @logradouro, @numero, @cidade, @bairro, @complemento, @estado, @fotoperfil, @biografia)";
            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@nome", ong.Nome);
            comando.Parameters.AddWithValue("@celular", ong.Celular);
            comando.Parameters.AddWithValue("@email", ong.Email);
            comando.Parameters.AddWithValue("@senha", ong.Senha);
            comando.Parameters.AddWithValue("@cep", ong.Cep);
            comando.Parameters.AddWithValue("@logradouro", ong.Logradouro);
            comando.Parameters.AddWithValue("@numero", ong.Numero);
            comando.Parameters.AddWithValue("@cidade", ong.Cidade);
            comando.Parameters.AddWithValue("@bairro", ong.Bairro);
            comando.Parameters.AddWithValue("@complemento", ong.Complemento);
            comando.Parameters.AddWithValue("@estado", ong.Estado);
            comando.Parameters.AddWithValue("@fotoperfil", ong.FotoPerfil);
            comando.Parameters.AddWithValue("@biografia", ong.Biografia);
            comando.ExecuteNonQuery();
            conexao.Close();
        }

        public List<ONGDTO> ListarInfoONGs()
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();
            var query = "SELECT * FROM ONGs";
            var comando = new MySqlCommand(query, conexao);
            var dataReader = comando.ExecuteReader();
            var comandos = new List<ONGDTO>();
            while (dataReader.Read())
            {
                var ONG = new ONGDTO();
                ONG.ID = int.Parse(dataReader["ID"].ToString());
                ONG.Nome = dataReader["Nome"].ToString();
                ONG.Celular = dataReader["Celular"].ToString();
                ONG.Email = dataReader["Email"].ToString();
                ONG.Cep = dataReader["Cep"].ToString();
                ONG.Logradouro = dataReader["Logradouro"].ToString();
                ONG.Numero = dataReader["Numero"].ToString();
                ONG.Cidade = dataReader["Cidade"].ToString();
                ONG.Bairro = dataReader["Bairro"].ToString();
                ONG.Complemento = dataReader["Complemento"].ToString();
                ONG.FotoPerfil = dataReader["FotoPerfil"].ToString();
                ONG.Biografia = dataReader["Biografia"].ToString();
                comandos.Add(ONG);
            }
            conexao.Close();
            return comandos;
        }

        public bool VerificarONG(ONGDTO ong)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();
            var query = "SELECT * FROM ONGs WHERE email = @email";
            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@email", ong.Email);
            var dataReader = comando.ExecuteReader();
            var ongs = new List<ONGDTO>();
            while (dataReader.Read())
            {
                ong = new ONGDTO();
                ong.ID = int.Parse(dataReader["ID"].ToString());
                ong.Email = dataReader["Email"].ToString();
                ong.Senha = dataReader["Senha"].ToString();
                ongs.Add(ong);
            }
            conexao.Close();
            return ongs.Count > 0;
        }

        public ONGDTO Login(ONGDTO ong)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();
            var query = "SELECT * FROM ONGs WHERE email = @email and senha = @senha";
            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@email", ong.Email);
            comando.Parameters.AddWithValue("@senha", ong.Senha);
            var dataReader = comando.ExecuteReader();
            ong = new ONGDTO();
            while (dataReader.Read())
            {
                ong.ID = int.Parse(dataReader["ID"].ToString());
                ong.Email = dataReader["Email"].ToString();
                ong.Senha = dataReader["Senha"].ToString();
            }
            conexao.Close();
            return ong;
        }

        public ONGDTO ListarOngsPorID(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();
            var query = "SELECT * FROM ONGs Where ID = @id";
            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);
            var dataReader = comando.ExecuteReader();
            var ong = new ONGDTO();
            while (dataReader.Read())
            {
                ong.ID = int.Parse(dataReader["ID"].ToString());
                ong.Nome = dataReader["Nome"].ToString();
                ong.Celular = dataReader["Celular"].ToString();
                ong.Email = dataReader["Email"].ToString();
                ong.Senha = dataReader["Senha"].ToString();
                ong.Cep = dataReader["Cep"].ToString();
                ong.Logradouro = dataReader["Logradouro"].ToString();
                ong.Numero = dataReader["Numero"].ToString();
                ong.Cidade = dataReader["Cidade"].ToString();
                ong.Bairro = dataReader["Bairro"].ToString();
                ong.Complemento = dataReader["Complemento"].ToString();
                ong.Estado = dataReader["Estado"].ToString();
                ong.FotoPerfil = dataReader["FotoPerfil"].ToString();
                ong.Biografia = dataReader["Biografia"].ToString();
            }
            conexao.Close();
            return ong;
        }
        public void Atualizar(ONGDTO ong)
        {
            using (var conexao = ConnectionFactory.Build())
            {
                conexao.Open();
                var query = @"UPDATE ONGs SET 
                        Nome = @nome,
                        Celular = @celular,
                        Email = @email,
                        Senha = @senha,
                        FotoPerfil = @fotoperfil
                      WHERE ID = @id";
                using (var comando = new MySqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@id", ong.ID);
                    comando.Parameters.AddWithValue("@nome", ong.Nome);
                    comando.Parameters.AddWithValue("@celular", ong.Celular);
                    comando.Parameters.AddWithValue("@email", ong.Email);
                    comando.Parameters.AddWithValue("@senha", ong.Senha);
                    comando.Parameters.AddWithValue("@fotoperfil", ong.FotoPerfil);
                    comando.ExecuteNonQuery();
                }
            }
        }

    }
}
