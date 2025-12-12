using SOAP.Models;
using System;
using System.Data;
using System.Data.SqlClient;


namespace SOAP.Repository
{
    public class UtilizadorRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UtilizadorRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int AddUser(Utilizador user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "O utilizador não pode ser nulo");

            using (SqlConnection conn = _connectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand("RegistrarUtilizador", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Auth0UserId", user.Auth0UserId);
                cmd.Parameters.AddWithValue("@Nome", user.Nome);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@ImgUrl", user.ImgUrl);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()); 
            }
        }
    }
}