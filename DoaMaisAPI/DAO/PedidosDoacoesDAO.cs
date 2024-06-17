using DoaMaisAPI.Controllers;
using DoaMaisAPI.DTO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace DoaMaisAPI.DAO
{
    public class PedidosDoacoesDAO
    {
        public void CadastrarPedidos(PedidoDoacaoDTO Pedidos)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO PedidosDoacao (Titulo, ID_Tipo, Descricao, ID_ONG, Status) 
                         VALUES (@titulo, @id_tipo, @descricao, @id_ong, @status);
                         SELECT LAST_INSERT_ID();";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@titulo", Pedidos.Titulo);
            comando.Parameters.AddWithValue("@id_tipo", Pedidos.ID_Tipo);
            comando.Parameters.AddWithValue("@descricao", Pedidos.Descricao);
            comando.Parameters.AddWithValue("@id_ong", Pedidos.ID_ONG);
            comando.Parameters.AddWithValue("@status", Pedidos.Status);

            var idPedido = Convert.ToInt32(comando.ExecuteScalar());
            conexao.Close();

            CadastrarImagensPedido(idPedido, Pedidos.ImagensPedido);
        }
        public List<PedidoDoacaoDTO> ListarPedidosDoacao()
        {
            var pedidos = new List<PedidoDoacaoDTO>();
            var ongDao = new ONGDAO();

            using (var conexao = ConnectionFactory.Build())
            {
                conexao.Open();

                var query = @"SELECT 
                                p.* , o.Nome,o.Celular,o.Email,o.Senha,o.Cep,o.Logradouro,
                                o.Numero,o.Cidade,o.Bairro,o.Complemento,o.Estado,
                                o.FotoPerfil,o.Biografia 
                                FROM PedidosDoacao p
                                INNER JOIN ONGs o
                                ON p.ID_ONG = o.ID
                                WHERE Status = 1";

                using (var comando = new MySqlCommand(query, conexao))
                using (var dataReader = comando.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var pedido = new PedidoDoacaoDTO
                        {
                            ID = dataReader.GetInt32("ID"),
                            Titulo = dataReader.GetString("Titulo"),
                            Descricao = dataReader.GetString("Descricao"),
                            ID_ONG = dataReader.GetInt32("ID_ONG"),
                            Status = dataReader.GetInt32("Status") == 1,
                            ImagensPedido = ListarImagensPedido(dataReader.GetInt32("ID"))
                        };


                        var ong = new ONGDTO();
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

                        pedido.ONG = ong;

                        pedidos.Add(pedido);
                    }
                }
            }

            return pedidos;
        }

        public void InativarPedidoDoacao(int idPedido)
        {
            using (var conexao = ConnectionFactory.Build())
            {
                conexao.Open();

                var query = "UPDATE PedidosDoacao SET Status = 0 WHERE ID = @idPedido";
                using (var comando = new MySqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@idPedido", idPedido);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void ReativarPedidoDoacao(int idPedido)
        {
            using (var conexao = ConnectionFactory.Build())
            {
                conexao.Open();

                var query = "UPDATE PedidosDoacao SET Status = 1 WHERE ID = @idPedido";
                using (var comando = new MySqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@idPedido", idPedido);
                    comando.ExecuteNonQuery();
                }
            }
        }


        private List<ImagemPedidoDoacaoDTO> ListarImagensPedido(int idPedido)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var imagens = new List<ImagemPedidoDoacaoDTO>();

            var query = "SELECT Link FROM ImagensPedidoDoacao WHERE ID_PedidoDoacao = @idPedido";
            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@idPedido", idPedido);
            var dataReader = comando.ExecuteReader();

            while (dataReader.Read())
            {
                var imagem = new ImagemPedidoDoacaoDTO();
                imagem.Link = dataReader["Link"].ToString();
                imagens.Add(imagem);
            }

            conexao.Close();

            return imagens;
        }

        public void CadastrarImagensPedido(int idPedido, List<ImagemPedidoDoacaoDTO> imagens)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"INSERT INTO ImagensPedidoDoacao (Link, ID_PedidoDoacao) 
                         VALUES (@link, @pedido);";

            foreach (var imagem in imagens)
            {
                var comando = new MySqlCommand(query, conexao);
                comando.Parameters.AddWithValue("@link", imagem.Link);
                comando.Parameters.AddWithValue("@pedido", idPedido);

                comando.ExecuteNonQuery();
            }
            conexao.Close();
        }

        public PedidoDoacaoDTO ListarRequisicaoPorID(int id)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = "SELECT * FROM PedidosDoacao WHERE ID = @id";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@id", id);

            var dataReader = comando.ExecuteReader();

            var ong = new PedidoDoacaoDTO();

            while (dataReader.Read())
            {
                ong.ID = int.Parse(dataReader["ID"].ToString());
                ong.Titulo = dataReader["Titulo"].ToString();
                ong.ID_Tipo = int.Parse(dataReader["ID_Tipo"].ToString());
                ong.Descricao = dataReader["Descricao"].ToString();
                ong.ID_ONG = int.Parse(dataReader["ID_ONG"].ToString());
                ong.Status = Convert.ToInt32(dataReader["Status"]) == 1;
            }
            conexao.Close();

            return ong;
        }
        public List<PedidoDoacaoDTO> ListarInativos(int ongId)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"SELECT 
                            P.ID, P.Titulo, P.Descricao, P.ID_Tipo, P.ID_ONG, P.Status,
                            O.Nome, O.Cidade, O.Estado, O.FotoPerfil
                            FROM PedidosDoacao P
                            INNER JOIN Ongs O
                            ON P.ID_ONG = O.ID
                            WHERE P.ID_ONG = @ongId AND P.Status = 0;";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@ongId", ongId);
            var dataReader = comando.ExecuteReader();

            var pedidos = new List<PedidoDoacaoDTO>();

            while (dataReader.Read())
            {
                var pedido = new PedidoDoacaoDTO();
                pedido.ID = int.Parse(dataReader["ID"].ToString());
                pedido.Titulo = dataReader["Titulo"].ToString();
                pedido.Descricao = dataReader["Descricao"].ToString();
                pedido.ID_ONG = int.Parse(dataReader["ID_ONG"].ToString());
                pedido.Status = Convert.ToInt32(dataReader["Status"]) == 1;

                var ong = new ONGDTO();
                ong.ID = pedido.ID_ONG.Value;
                ong.Nome = dataReader["Nome"].ToString();
                ong.Cidade = dataReader["Cidade"].ToString();
                ong.Estado = dataReader["Estado"].ToString();
                ong.FotoPerfil = dataReader["FotoPerfil"].ToString();

                pedido.ONG = ong;

                pedido.ImagensPedido = ListarImagensPedido(pedido.ID);

                pedidos.Add(pedido);
            }

            conexao.Close();

            return pedidos;
        }
        public List<PedidoDoacaoDTO> ListarPedidosDoacaoPorONG(int ongId)
        {
            var conexao = ConnectionFactory.Build();
            conexao.Open();

            var query = @"SELECT 
                            P.ID, P.Titulo, P.Descricao, P.ID_Tipo, P.ID_ONG, P.Status,
                            O.Nome, O.Cidade, O.Estado, O.FotoPerfil
                            FROM PedidosDoacao P
                            INNER JOIN Ongs O
                            ON P.ID_ONG = O.ID
                            WHERE P.ID_ONG = @ongId AND P.Status = 1;";

            var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@ongId", ongId);
            var dataReader = comando.ExecuteReader();

            var pedidos = new List<PedidoDoacaoDTO>();

            while (dataReader.Read())
            {
                var pedido = new PedidoDoacaoDTO();
                pedido.ID = int.Parse(dataReader["ID"].ToString());
                pedido.Titulo = dataReader["Titulo"].ToString();
                pedido.Descricao = dataReader["Descricao"].ToString();
                pedido.ID_ONG = int.Parse(dataReader["ID_ONG"].ToString());
                pedido.Status = Convert.ToInt32(dataReader["Status"]) == 1;

                // Listar imagens para este pedido
                pedido.ImagensPedido = ListarImagensPedido(pedido.ID);

                var ong = new ONGDTO();
                ong.ID = pedido.ID_ONG.Value;
                ong.Nome = dataReader["Nome"].ToString();
                ong.Cidade = dataReader["Cidade"].ToString();
                ong.Estado = dataReader["Estado"].ToString();
                ong.FotoPerfil = dataReader["FotoPerfil"].ToString();

                pedido.ONG = ong;

                pedidos.Add(pedido);
            }

            conexao.Close();

            return pedidos;
        }

    }
}
