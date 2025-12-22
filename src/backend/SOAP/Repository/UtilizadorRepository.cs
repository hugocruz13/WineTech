using SOAP.Models;
using System;
using System.Collections.Generic;
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

        public Utilizador AddUser(Utilizador user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "O utilizador não pode ser nulo");

            using (SqlConnection conn = _connectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand("RegistrarUtilizador", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Auth0UserId", user.Id);
                cmd.Parameters.AddWithValue("@Nome", user.Nome);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@ImgUrl", user.ImgUrl);
                cmd.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Utilizador
                        {
                            Id = reader["Id"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            ImgUrl = reader["ImgUrl"].ToString(),
                        };
                    }
                }
                return null;
            }
        }


        public Utilizador GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("O ID do utilizador não pode ser nulo ou vazio", nameof(id));
            using (SqlConnection conn = _connectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand("UtilizadorById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Auth0UserId", id);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Utilizador
                        {
                            Id = reader["Id"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            ImgUrl = reader["ImgUrl"].ToString(),
                        };
                    }
                }
                return null;
            }
        }

        public List<Utilizador> GetOwners()
        {
            List<Utilizador> users = new List<Utilizador>();
            using (SqlConnection conn = _connectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand("UtilizadoresAdmin", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new Utilizador
                        {
                            Id = reader["Id"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Email = reader["Email"].ToString(),
                            ImgUrl = reader["ImgUrl"].ToString(),
                            IsAdmin = Convert.ToBoolean(reader["IsAdmin"])
                        });
                    }
                }
            }
            return users;
        }
    }
}