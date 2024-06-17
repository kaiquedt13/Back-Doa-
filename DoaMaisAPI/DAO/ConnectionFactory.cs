using MySql.Data.MySqlClient;

namespace DoaMaisAPI.DAO
{
    public class ConnectionFactory
    {
        public static MySqlConnection Build()
        {
            return new MySqlConnection("Server=localhost;Database=DoaMais;Uid=root;Pwd=root;");
        }
    }
}
